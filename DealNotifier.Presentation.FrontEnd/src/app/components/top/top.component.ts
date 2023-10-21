import { AuthService } from './../../services/auth.service';
import { Subscription } from 'rxjs';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ItemService } from '../../services/item.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-top',
  templateUrl: './top.component.html',
  styleUrls: ['./top.component.css'],
  //changeDetection: ChangeDetectionStrategy.OnPush
})
export class TopComponent implements OnInit, OnDestroy {
  total$: number;
  search$: string;
  SubscriptionList: Subscription[] = [];
  constructor(
    public itemService: ItemService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    const subs1 = this.itemService.total$.subscribe((data) => {
      this.total$ = data;
    });

    const subs2 = this.itemService.serch$.subscribe((data) => {
      this.search$ = data;
    });

    this.SubscriptionList.push(subs1);
    this.SubscriptionList.push(subs2);
  }

  ngOnDestroy(): void {
    this.SubscriptionList.forEach((i) => i.unsubscribe());
  }
  searching(query: any) {
    if (query != '') {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { search: query },
      });
    } else {
      this.router.navigate([], {
        relativeTo: this.route,
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['login']);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
}
