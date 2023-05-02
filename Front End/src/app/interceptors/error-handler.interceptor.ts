import { ErrorHandlerService } from './../services/error-handler.service';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {
  constructor(private errorHandlerService: ErrorHandlerService) { }

  intercept(request: HttpRequest<JSON>, next: HttpHandler): Observable<HttpEvent<JSON>> {
    return next.handle(request).pipe(
      tap({
        error: (error => {
          this.errorHandlerService.ShowError(error);
        })
      })
    );
  }
}
