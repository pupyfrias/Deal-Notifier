import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Item} from '../models/item';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  total$ = new BehaviorSubject<number>(0);
  select$ = new BehaviorSubject<number>(0);
  serch$ = new BehaviorSubject<string>('');
  data$ = new BehaviorSubject<Item[]>({} as Item[]);

  constructor(private httpClient: HttpClient) { }

  GetRequest(): Observable<Item[]> {
    const api = environment.baseApi + 'items/';
    return this.httpClient.get<Item[]>( api + location.search);
  }

  Login(data: JSON): Observable<any> {
    const api = environment.baseApi + 'account/authenticate';
    return this.httpClient.post(api, data, { responseType: 'text' });

  }

   

}
