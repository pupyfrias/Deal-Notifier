import { TokenInterceptor } from './interceptors/token.interceptor';
import { SpinerInterceptor } from './interceptors/spiner.interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatRadioModule } from '@angular/material/radio';
import { MatInputModule } from '@angular/material/input';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ItemComponent } from './components/item/item.component';
import { sidenavComponent } from './components/sidenav/sidenav.component';
import { TopComponent } from './components/top/top.component';
import { DialogComponent } from './components/dialog/dialog.component';
import { StoresPipe } from './pipes/stores.pipe';
import { ConditionPipe } from './pipes/condition.pipe';
import { ContainerComponent } from './components/container/container.component';
import { ErrorHandlerInterceptor } from './interceptors/error-handler.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ContainerComponent,
    ItemComponent,
    sidenavComponent,
    TopComponent,
    DialogComponent,
    StoresPipe,
    ConditionPipe
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
  exports: [sidenavComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: SpinerInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
