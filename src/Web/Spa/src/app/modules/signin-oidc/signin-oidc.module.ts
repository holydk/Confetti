import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignInOidcRoutes } from './signin-oidc.routing';
import { SignInOidcComponent } from './pages/signin-oidc/signin-oidc.component';

@NgModule({
  declarations: [SignInOidcComponent],
  imports: [
    CommonModule,
    SignInOidcRoutes
  ]
})
export class SignInOidcModule { }
