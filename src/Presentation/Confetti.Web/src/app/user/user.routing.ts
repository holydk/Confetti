import { Routes, RouterModule } from '@angular/router';

import {
  WrapperComponent as UserWrapperComponent
 } from '@user/shared/layouts/wrapper/components/wrapper.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: './modules/home/home.module#HomeModule',
    component: UserWrapperComponent
  },
  {
    path: '**',
    loadChildren: './modules/not-found/not-found.module#NotFoundModule',
    component: UserWrapperComponent
  }
];

export const UserRoutes = RouterModule.forChild(routes);
