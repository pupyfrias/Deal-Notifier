import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt'
import { TokenService } from '../services/token.service';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  jwtHelper = new JwtHelperService();

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
  }

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean> {
    let isAuthenticated = this.authService.isAuthenticated();
    if (isAuthenticated) {
      return true;
    }
    this.router.navigate(['login']);
    return false;
  }
}
