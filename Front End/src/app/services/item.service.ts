import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ResponseDTO } from '../models/ResponseDTO';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  total$ = new BehaviorSubject<number>(0);
  select$ = new BehaviorSubject<number>(0);
  serch$ = new BehaviorSubject<string>('');
  // data$ = new BehaviorSubject<Item[]>({} as Item[]);

  constructor(private httpClient: HttpClient) { }

  GetRequest(): Observable<ResponseDTO> {
    const api = environment.baseApi + 'items/';
    return this.httpClient.get<ResponseDTO>(api + location.search);
  }

  Login(body: JSON): Observable<ResponseDTO> {
    const api = environment.baseApi + 'account/login';

    return this.httpClient.post<ResponseDTO>(api, body);
  }
}
