import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import {AccordionModule, } from 'ngx-bootstrap/accordion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { NgxSpinnerModule } from "ngx-spinner";
import {NgxPaginationModule} from 'ngx-pagination';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatMenuModule} from '@angular/material/menu';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatCardModule} from '@angular/material/card';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatCheckboxModule} from '@angular/material/checkbox'
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatBadgeModule} from '@angular/material/badge';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatRadioModule} from '@angular/material/radio';
import {MatInputModule} from '@angular/material/input';




import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ItemComponent } from './components/item/item.component';
import { sidenavComponent } from './components/sidenav/sidenav.component';
import { TopComponent } from './components/top/top.component';
import { TitleCasePipe } from '@angular/common';
import { DialogComponent } from './components/dialog/dialog.component';



@NgModule({
  declarations: [
    AppComponent,
    ItemComponent,
    sidenavComponent,
    TopComponent,
    DialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ToastrModule.forRoot(),
    AccordionModule.forRoot(),
    BrowserAnimationsModule,
    FormsModule,
    ScrollingModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    MatGridListModule,
    MatMenuModule,
    MatSidenavModule,
    MatCardModule,
    MatToolbarModule,
    MatSlideToggleModule,
    MatCheckboxModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule,
    MatExpansionModule,
    MatRadioModule,
    MatInputModule,
    MatCardModule
  ],
  providers: [TitleCasePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
