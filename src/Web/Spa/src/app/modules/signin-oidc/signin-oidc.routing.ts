import { Routes, RouterModule } from '@angular/router';
import { SignInOidcComponent } from './pages/signin-oidc/signin-oidc.component';

const routes: Routes = [
  {
    path: '',
    component: SignInOidcComponent
  }
];

export const SignInOidcRoutes = RouterModule.forChild(routes);
