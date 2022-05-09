import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError, BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { item } from '../models/item';
import { environment } from 'src/environments/environment';
import { AES,enc }  from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  total$ = new BehaviorSubject<number>(0);
  select$ = new BehaviorSubject<number>(0);
  serch$ = new BehaviorSubject<string>('');
  data$ = new BehaviorSubject<item[]>({} as item[]);

  constructor(private httpClient: HttpClient, private toastr: ToastrService) {}

  GetRequest(): Observable<any> {
    const api = environment.baseApi + 'items/' + location.search;
    return this.httpClient.get(api).pipe(catchError(this.HandlerError));
  }

  Login(data: JSON): Observable<any> {
    const api = environment.baseApi + 'login';

    return this.httpClient.post(api, data, { responseType: 'text' });
  }

  HandlerError(error: HttpErrorResponse) {
    return throwError(error.status);
  }

  Encrypt(data: string): string {
    try{
      return AES.encrypt(data, environment.encryptKey).toString();
    }
    catch{
      return '';
    }
    
  }

  Decrypt(data: string): string {
    try{
      return AES.decrypt(data, environment.encryptKey)
      .toString(enc.Utf8);
    }
    catch{
      return '';
    }
   
  }

  ShowError(error: any) {
    if (error) {
      if (error === 401) {
        this.toastr.warning('Expired Session');
      }
      else if (error === 404) {
        this.toastr.error('404');
      } else if (error === 409) {
        this.toastr.info('409');
      } else {
        this.toastr.info('Ocurrió un error en el servidor');
      }
    } else if (error === 0) {
      this.toastr.error('Off line');
    }
  }
}
