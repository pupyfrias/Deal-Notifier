import { ItemService } from 'src/app/services/item.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  
  constructor(
    private service: ItemService,
    private router: Router
    ){

  }
  
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {

      const encryptToken = sessionStorage.getItem('token');
      if(encryptToken){
        const decryptToken = this.service.Decrypt(encryptToken);
        if(decryptToken.length> 0)
        {
          return true;
        }
      }
    
    this.router.navigateByUrl('/login');
    return false;
  }
}
