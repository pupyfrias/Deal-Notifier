import { Component } from "@angular/core"
import { OnInit } from "@angular/core"
import { ItemService } from "src/app/services/item.service"

@Component({
    selector: 'app-dialog',
    templateUrl: './dialog.component.html',
})
export class DialogComponent implements OnInit {

    selected: number;
    constructor(private service: ItemService) {

    }
    ngOnInit(): void {
        this.service.select.subscribe(data => {
            this.selected = data;
        })
    }

}
