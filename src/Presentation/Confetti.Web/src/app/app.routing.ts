import { Routes, RouterModule } from '@angular/router';
// import { MetaGuard } from '@ngx-meta/core';

const routes: Routes = [
  {
    path: 'admin',
    loadChildren: './admin/admin.module#AdminModule'
  },
  {
    path: '',
    loadChildren: './user/user.module#UserModule'
  }
];

// must use {initialNavigation: 'enabled'}) - for one load page, without reload
export const AppRoutes = RouterModule.forRoot(routes, { initialNavigation: 'enabled' });
