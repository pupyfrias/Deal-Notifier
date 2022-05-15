import { NgxSpinnerService } from 'ngx-spinner';
import { catchError } from 'rxjs/operators';
import { item } from './../../models/item';
import {
  Component,
  OnInit,
  OnDestroy,
  ViewChildren,
  QueryList,
  AfterViewChecked,
  ChangeDetectorRef,
  ViewChild,
} from '@angular/core';
import { ItemService } from '../../services/item.service';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { MatCheckbox } from '@angular/material/checkbox';
import { environment } from 'src/environments/environment';
import { MatSidenavContent } from '@angular/material/sidenav';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css'],
})
export class ItemComponent implements OnInit, OnDestroy, AfterViewChecked {
  @ViewChildren(MatCheckbox) checkBoxes: QueryList<MatCheckbox>;
  @ViewChild('SideNav') sideNav: any;

  list: item[];
  date: Date = new Date();
  listIds: any[] = [];
  selected: number = 0;
  subscriptionList: Subscription[] = [];
  page: number = 1;
  checkAll = false;

  constructor(
    public itemService: ItemService,
    private route: ActivatedRoute,
    private httpClient: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private changeDetectorRef: ChangeDetectorRef,
    private NgxSpinnerService: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    // const subscription  = this.route.parent?.data.subscribe((data) => {
    //   this.list = data['items'];
    //   this.itemService.total$.next(this.list.length);
    //   this.page = 1;
    //   this.CleanAllCheckboxes();
    // });
    this.route.queryParams.subscribe(() => {
      const subscription = this.itemService.GetRequest().subscribe({
        next: (data) => {
          this.list = data;
          this.itemService.total$.next(this.list.length);
          this.page = 1;
          this.CleanAllCheckboxes();
        },
      });

      this.subscriptionList.push(subscription);
    });

    const subscription2 = this.itemService.select$.subscribe(
      (data) => (this.selected = data)
    );
    this.subscriptionList.push(subscription2);
  }

  ngOnDestroy(): void {
    this.subscriptionList.forEach((i) => {
      i.unsubscribe();
    });
    console.log('destroyed');
  }

  ngAfterViewChecked(): void {
    let counter = 0;
    this.checkBoxes.forEach((i) => {
      const element = i._elementRef.nativeElement;
      const name: string = element.getAttribute('name');
      const checked: string = element.classList.contains(
        'mat-checkbox-checked'
      );

      if (name === 'item' && checked) {
        ++counter;
      }
    });

    if (counter === 40) {
      this.checkAll = true;
    } else {
      this.checkAll = false;
    }
    this.changeDetectorRef.detectChanges();

    const sideNavElement = this.sideNav?.elementRef?.nativeElement;
    //console.log(this.sideNav);

    // this.sideNav.forEach(i=>{
    //   console.log('sideNav',i);
    // })
  }

  public onPageChange(): void {
    document.querySelector('mat-sidenav-content')?.scroll(0, 0);
  }

  //#region Delete
  ListDelete(e: any) {
    const id = e.source.value;
    this.SetListIds(e.checked, id);
    this.itemService.select$.next(this.listIds.length);
  }

  Delete() {
    if (this.listIds.length > 0) {
      this.itemService.select$.next(this.listIds.length);
      const dialogRef = this.dialog.open(DialogComponent);
      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          const api = environment.baseApi;

          this.httpClient
            .delete(`${api}items/?delete=${this.listIds}`)
            .subscribe({
              error: (error) => {
                this.itemService.ShowError(catchError(error));
                this.NgxSpinnerService.hide();
              },
              complete: () => {
                this.Reload();
                this.toastr.success(`${this.listIds.length} Item deleted`);
                this.listIds = [];
              },
            });

          this.itemService.select$.next(0);
        }
      });
    }
  }
  //#endregion

  //#region  reload
  Reload() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['./'], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }
  //#endregion

  //#region Select
  Select(e: any) {
    this.Checking(e.checked);
    this.itemService.select$.next(this.listIds.length);
  }
  //#endregion

  SetListIds(bool: boolean, id: string): void {
    if (bool) {
      if (!this.listIds.includes(id)) {
        this.listIds.push(id);
      }
    } else {
      const index = this.listIds.indexOf(id, 0);
      if (index > -1) {
        this.listIds.splice(index, 1);
      }
    }
  }

  Checking(bool: boolean): void {
    this.checkBoxes.forEach((i) => {
      const element = i._elementRef.nativeElement;
      const name = element.getAttribute('name');
      const id = element.getAttribute('id');
      if (name === 'item') {
        this.SetListIds(bool, id);
      }
    });
  }

  CleanAllCheckboxes(): void {
    this.listIds = [];
    this.itemService.select$.next(0);
  }
}
