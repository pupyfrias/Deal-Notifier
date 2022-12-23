import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CryptService } from '../services/crypt.service';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  jwtHelper = new JwtHelperService();

  constructor(
    private cryptService: CryptService,
    private router: Router,
    private tokenService: TokenService
  ) {
  }

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean> {
    const accessToken = sessionStorage.getItem('accesstoken');
    const isTokenExpired = this.jwtHelper.isTokenExpired(accessToken);
    let isRefreshSuccess = true;
    if (accessToken && !isTokenExpired) {
      console.log(this.jwtHelper.decodeToken(accessToken))
      return true;
    }

    isRefreshSuccess = await this.tokenService.tryRefreshingTokens(accessToken);
    if (!isRefreshSuccess) {
      this.router.navigate(["login"]);
    }

    return isRefreshSuccess;
  }
}
