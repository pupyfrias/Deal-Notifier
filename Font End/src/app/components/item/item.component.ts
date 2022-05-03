import { Component, OnInit, OnDestroy, ViewChildren, QueryList, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { item } from '../../models/item'
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from "ngx-spinner";
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';
import { Global } from 'src/assets/global';
import { } from '@angular/core';
import { MatCheckbox } from '@angular/material/checkbox';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit, OnDestroy, AfterViewChecked {

  @ViewChildren(MatCheckbox) checkBoxes: QueryList<MatCheckbox>;

  list: item[];
  date: Date = new Date();
  listIds: any[] = [];
  selected: number = 0;
  subscription: Subscription;
  page: number = 1;
  checkAll = false;


  constructor(
    public itemService: ItemService,
    private route: ActivatedRoute,
    private httpClient: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private dialog: MatDialog,
    private changeDetectorRef: ChangeDetectorRef
  ) { }




  ngOnInit(): void {

    this.route.queryParams.subscribe(() => {
      this.spinner.show();
      this.subscription = this.itemService.GetRequest().subscribe({
        next: (data: item[]) => {
          this.list = data;
          this.itemService.total.next(this.list.length);
          this.page = 1;
          this.CleanAllCheckboxes();
        },
        error: (error) => {
          this.itemService.ShowError(error);
          this.spinner.hide();
        },
        complete: () => {
          this.spinner.hide();
        }
      });

      this.itemService.select.subscribe(data => {
        this.selected = data
      });
    });
  }

  ngOnDestroy(): void {
    console.log('destroy');
    this.subscription.unsubscribe();
  }

  ngAfterViewChecked(): void {

    //Checking all checkeboxes are checked
    let counter = 0;
    this.checkBoxes.forEach(i => {
      const element = i._elementRef.nativeElement;
      const name: string = element.getAttribute('name');
      const checked: string = element.classList.contains("mat-checkbox-checked")

      if (name === 'item' && checked) {
        ++counter;
      }
    });

    if (counter === 40) {
      this.checkAll = true;
    }
    else {
      this.checkAll = false;
    }
    this.changeDetectorRef.detectChanges();
  }

  //#region On Page Change
  public onPageChange(): void {
    document.querySelector('mat-sidenav-content')?.scroll(0, 0);
  }
  //#endregion

  //#region Parse Date
  parseDate(date: Date) {

    let endDate = new Date();
    let purchaseDate = new Date(date);
    let diffMs = (endDate.getTime() - purchaseDate.getTime()); // milliseconds
    let diffDays = Math.floor(diffMs / 86400000); // days
    let diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
    let diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
    return diffDays + " days, " + diffHrs + " hours, " + diffMins + "minutes";

  }
  //#endregion 

  //#region  List Delete
  ListDelete(e: any) {
 
    const id = e.source.value;
    this.SetListIds(e.checked, id);
    this.itemService.select.next(this.listIds.length);
  }
  //#endregion

  //#region Delete
  Delete() {
    if (this.listIds.length > 0) {
      this.itemService.select.next(this.listIds.length);
      const dialogRef = this.dialog.open(DialogComponent);
      dialogRef.afterClosed().subscribe(result => {
        if (result) {

          const api = Global.baseApi;
          this.spinner.show();

          let promises: any = [];
          this.listIds.forEach((id) => {
            promises.push(new Promise(async (resolve, rejects) => {

              this.httpClient.delete(api + id)
                .subscribe({
                  error: () => { rejects(); },
                  complete: () => { resolve('done'); }
                });
            })
            );

          });

          Promise.all(promises).then(() => {
            this.Reload();
            this.toastr.success(`${this.listIds.length} Item deleted`);
            this.listIds = [];
          }).catch(() => {

            this.toastr.error("ocurrend an error");
            this.spinner.hide();

          });
          this.itemService.select.next(0);
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
      queryParamsHandling: 'preserve'
    });
  }
  //#endregion

  //#region Select
  Select(e: any) {

    this.Checking(e.checked);
    this.itemService.select.next(this.listIds.length);
  }
  //#endregion

  SetListIds(bool: boolean, id: string):void {
    if (bool) {

      if (!this.listIds.includes(id)) {
        this.listIds.push(id);
      }
    }
    else {
      const index = this.listIds.indexOf(id, 0);
      if (index > -1) {
        this.listIds.splice(index, 1);
      }
    }
  }

  Checking(bool: boolean):void {
    this.checkBoxes.forEach(i => {
      const element = i._elementRef.nativeElement;
      const name = element.getAttribute('name');
      const id = element.getAttribute('id');     
      if (name === 'item') {
        this.SetListIds(bool, id);
        
      }
    });
  }

  CleanAllCheckboxes():void{
    this.listIds = [];
    this.itemService.select.next(0);
  }
}

