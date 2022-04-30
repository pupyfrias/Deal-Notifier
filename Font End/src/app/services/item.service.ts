import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { throwError, observable, BehaviorSubject, Observable } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { item } from '../models/item';




@Injectable({
  providedIn: 'root'
})
export class ItemService {

  total = new BehaviorSubject<number>(0)
  select = new BehaviorSubject<number>(0)
  serch = new BehaviorSubject<string>("")
  data = new BehaviorSubject<item[]>({} as item[])

  constructor(
    private httpClient: HttpClient,
    private toastr: ToastrService,
  ) { }

  GetRequest():Observable<any> {


    let api = 'http://localhost:59573/api/Items/' + location.search;
    //let api = 'http://webscraping.com:8045/api/Items/' + location.search;
    return this.httpClient.get(api).pipe(catchError(this.HandlerError));

  }

  HandlerError(error: HttpErrorResponse) {
    return throwError(error.status);
  }

  ShowError(error: any) {

    if (error) {
      if (error === 404) {
        this.toastr.error("404")
      }
      else if (error === 409) {
        this.toastr.info("409")
      }
      else {
        this.toastr.info("Ocurrió un error en el servidor")
      }
    }
    else if (error === 0) {
      this.toastr.error("Off line")
    }
  }

}
