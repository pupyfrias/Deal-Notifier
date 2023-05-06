import { ResponseDTO } from './../models/ResponseDTO';
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { ErrorHandlerService } from '../services/error-handler.service';

@Injectable()
export class RefreshTokenInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(private authService: AuthService,
    private errorHandlerService: ErrorHandlerService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // if (this.authService.isAuthenticated()) {
    // }
    request = this.addAuthorizationHeader(request, this.authService.getAccessToken());
    return next.handle(request).pipe(
      catchError(error => {
        if (error.status === 401) {
          return this.handle401Error(request, next);
        } else {
          this.errorHandlerService.ShowError(error);
          return throwError(error);
        }
      })
    );
  }

  private addAuthorizationHeader(request: HttpRequest<any>, token: string | null): HttpRequest<any> {
    if (token) {
      return request.clone({
        withCredentials: true,
        setHeaders: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        }
      });
    }
    return request;
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      return this.authService.refreshToken()
        .pipe(
          switchMap((response: ResponseDTO) => {
            this.isRefreshing = false;
            console.log('response', response)
            if (response && response.data.accessToken) {
              this.authService.setAccessToken(response.data.accessToken);
              return next.handle(this.addAuthorizationHeader(request, response.data.accessToken));
            }

            this.errorHandlerService.ShowErrorString('No se pudo obtener un nuevo token de acceso');
            return throwError('No se pudo obtener un nuevo token de acceso');
          }),
          catchError(error => {
            //this.authService.logout();
            this.errorHandlerService.ShowError(error);
            return throwError(error);
          })
        );
    } else {
      return next.handle(request);
    }
  }
}
