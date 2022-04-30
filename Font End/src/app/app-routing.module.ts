import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ItemComponent} from 'src/app/components/item/item.component';

const routes: Routes = [
  {path:'',component: ItemComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
