import { ContainerComponent } from './components/container/container.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ItemComponent } from 'src/app/components/item/item.component';

const routes: Routes = [
  {
      path: '',
      pathMatch: 'full',
      component: ContainerComponent,
      children:[
        {
          path: '',
          pathMatch: 'full',
          component: ItemComponent,
          outlet: 'item'
        },
      ]
    },
    
  {
    path: 'login',
    loadChildren: () =>
      import('./components/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: '**',
    redirectTo: '/',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
