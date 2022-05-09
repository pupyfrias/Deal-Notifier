import { ItemsResolver } from './resolvers/items.resolver';
import { ContainerComponent } from './components/container/container.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ItemComponent } from 'src/app/components/item/item.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
      path: '',
      pathMatch: 'full',
      //resolve: ItemsResolver,
      component: ContainerComponent,
      canActivate: [AuthGuard],
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
