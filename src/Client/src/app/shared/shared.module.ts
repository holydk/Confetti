import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({
  exports: [],
  providers: [],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return { ngModule: SharedModule };
  }
}
