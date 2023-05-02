import {
  HttpEvent, HttpHandler, HttpInterceptor, HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor() { }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const accessToken = sessionStorage.getItem('accesstoken');


      let pushToke = request.clone({

        withCredentials: true,
        setHeaders: {
          Authorization:  `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        }

      });
      return next.handle(pushToke);
    
  }
}
