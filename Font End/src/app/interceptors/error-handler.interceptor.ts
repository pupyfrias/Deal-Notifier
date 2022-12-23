import { ErrorHandlerService } from './../services/error-handler.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';



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
