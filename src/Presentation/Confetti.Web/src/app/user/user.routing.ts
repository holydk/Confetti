import { Routes, RouterModule } from '@angular/router';

import { WrapperComponent } from '@user/layouts/wrapper/components/wrapper.component';

const routes: Routes = [
  {
    path: '',
    component: WrapperComponent,
    children: [
      {
        path: '',
        loadChildren: './modules/home/home.module#HomeModule'
      },
      {
        path: ':slug',
        loadChildren: './modules/home/home.module#HomeModule'
  },
  {
    path: '**',
        loadChildren: './modules/not-found/not-found.module#NotFoundModule'
      }
    ]
  }
];

export const UserRoutes = RouterModule.forChild(routes);
