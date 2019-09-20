import { AboutComponent } from './pages/about/about.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: AboutComponent
  }
];

export const AboutRoutes = RouterModule.forChild(routes);
