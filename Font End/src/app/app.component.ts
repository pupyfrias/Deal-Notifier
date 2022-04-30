import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {


  title = 'Frontend';

  onScrol(e: Event) {

    const element = document.getElementById("f-element");
    const scrollTop = document.getElementById("scrollToUp");
    let pagination = document.getElementById("pagination")!.offsetTop;
    const scroll = document.querySelector('mat-sidenav-content')!.scrollTop;


    if (element!.style.bottom === "50px") {
      pagination -= 60;
    }

    if (scroll! + 520 > pagination!) {

      element!.style.bottom = "50px";
    }
    else {
      element!.style.bottom = "0px";
    }

    if (scroll > 700) {
      scrollTop!.hidden = false;
    }
    else {

      scrollTop!.hidden = true;
    }


  }
}



