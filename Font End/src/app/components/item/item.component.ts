import { Component, OnInit, OnDestroy } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { item } from '../../models/item'
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from "ngx-spinner";
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';





@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit, OnDestroy {


  list: item[];
  date: Date = new Date();
  listIds: any[] = [];
  selected: number = 0;
  subscription: Subscription;
  page: number = 1;


  constructor(
    public itemService: ItemService,
    private route: ActivatedRoute,
    private httpClient: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private dialog: MatDialog
  ) { }



  ngOnInit(): void {

    this.route.queryParams.subscribe(() => {
      this.spinner.show();
      this.subscription = this.itemService.GetRequest().subscribe({
        next: (data: item[]) => {
          this.list = data;
          this.itemService.total.next(this.list.length);
          this.page = 1;
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


  public onPageChange(): void {

    document.querySelector('mat-sidenav-content')?.scroll(0, 0);
    /* 
 
     this.listIds.forEach(i => {
       var checkbox = document.querySelector(`input[type=checkbox][value='${i}']`)
       checkbox?.setAttribute("aria-checked", "true");
       var input = document.querySelector(`mat-checkbox[ng-reflect-value="${i}"]`);
       input?.classList.add("mat-checkbox-checked");
       console.log(checkbox);
     }); */

  }


  parseDate(date: Date) {

    let endDate = new Date();
    let purchaseDate = new Date(date);
    let diffMs = (endDate.getTime() - purchaseDate.getTime()); // milliseconds
    let diffDays = Math.floor(diffMs / 86400000); // days
    let diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
    let diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
    return diffDays + " days, " + diffHrs + " hours, " + diffMins + "minutes";

  }

  ListDelete(e: any) {

    console.log(e);
    const id = e.source.value
    if (e.checked === true) {
      this.listIds.push(id)
    }
    else {
      const index = this.listIds.indexOf(id, 0);
      if (index > -1) {
        this.listIds.splice(index, 1);
      }
    }

    this.itemService.select.next(this.listIds.length)
  }


  Delete() {
    if (this.listIds.length > 0) {
      
      this.itemService.select.next(this.listIds.length);
      const dialogRef = this.dialog.open(DialogComponent);

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          
          const api = "http://localhost:59573/api/Items/";
          // const api = "http://webscraping.com:8045/api/Items/"
          this.spinner.show();

          let promises: any = [];
          this.listIds.forEach(async (id) => {
            promises.push(new Promise(async (resolve, rejects) => {

              await this.httpClient.delete(api + id)
                .subscribe(() => { },
                  () => { rejects() },
                  () => { resolve('done') }
                );
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

  Reload() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['./'], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve'
    });
  }

  Select(e: any) {

    var checks = document.querySelectorAll("input[type=checkbox][name='item']")

    if (e.checked === true) {
      checks.forEach(item => {
        item.setAttribute("aria-checked", "true");
        var input = document.querySelector(`mat-checkbox[ng-reflect-value="${item.getAttribute("value")}"]`);
        input?.classList.add("mat-checkbox-checked");
        this.listIds.push(item.getAttribute("value"));
      });

    }
    else {
      checks.forEach(item => {
        if (item.className == "mat-checkbox-input cdk-visually-hidden") {
          item.removeAttribute("aria-checked");
          var input = document.querySelector(`mat-checkbox[ng-reflect-value="${item.getAttribute("value")}"]`);
          input?.classList.remove("mat-checkbox-checked");
        }
      });

      this.listIds = [];
    }

    this.itemService.select.next(this.listIds.length);
  }

}

