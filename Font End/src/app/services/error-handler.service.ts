import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor(private toastr: ToastrService) { }


  ShowError(error: HttpErrorResponse): void {
    this.toastr.error(error.status.toString(), error.error)
  }

}
