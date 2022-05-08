import { ItemService } from 'src/app/services/item.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class ItemsResolver implements Resolve<Observable<any>> {
  constructor(private service: ItemService) {}

  resolve() {
    return this.service.GetRequest();
  }
}
