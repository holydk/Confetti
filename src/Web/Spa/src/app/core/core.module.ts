import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { IdentityModule } from './identity/identity.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    IdentityModule
  ]
})
export class CoreModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        AuthService
      ]
    };
  }
}
