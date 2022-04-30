import { Component, OnInit } from '@angular/core';
import {ItemService} from '../../services/item.service'
import { ActivatedRoute,Router } from '@angular/router';

@Component({
  selector: 'app-top',
  templateUrl: './top.component.html',
  styleUrls: ['./top.component.css']
})
export class TopComponent implements OnInit {

  total:number;
  search : string;
  constructor(
    public itemService: ItemService,
    private route:ActivatedRoute,
    private router: Router
    ) { }

  ngOnInit( ): void {

    this.itemService.total.subscribe(data=>{
      this.total = data;
    });

    this.itemService.serch.subscribe(data=>{
      this.search = data;
    });
  }

  searching(query: any){

    if(query != ""){
      this.router.navigate([],{
        relativeTo: this.route,
        queryParams: {"search":query}
      })
    }
    else{
      this.router.navigate([],{
        relativeTo: this.route
      })
    }

  }
}
