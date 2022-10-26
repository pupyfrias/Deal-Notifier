import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { item } from '../models/item';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  total$ = new BehaviorSubject<number>(0);
  select$ = new BehaviorSubject<number>(0);
  serch$ = new BehaviorSubject<string>('');
  data$ = new BehaviorSubject<item[]>({} as item[]);

  constructor(private httpClient: HttpClient) { }

  GetRequest(): Observable<any> {
    const api = environment.baseApi + 'items/' + location.search;
    return this.httpClient.get(api);
  }

  Login(data: JSON): Observable<any> {
    const api = environment.baseApi + 'login';
    return this.httpClient.post(api, data, { responseType: 'text' });
  }

  


}
