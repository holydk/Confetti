import { Routes, RouterModule } from '@angular/router';

import {
  WrapperComponent as AdminWrapperComponent
 } from '@admin/shared/layouts/wrapper/components/wrapper.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: './modules/dashboard/dashboard.module#DashboardModule',
    component: AdminWrapperComponent
  },
  {
    path: '**',
    redirectTo: '',
    component: AdminWrapperComponent
  }
];

export const AdminRoutes = RouterModule.forChild(routes);
