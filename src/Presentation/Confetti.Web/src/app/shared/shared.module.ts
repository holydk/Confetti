import { ModuleWithProviders, NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { SharedEffects } from './state/effects/shared.effects';

@NgModule({
  imports: [
    EffectsModule.forFeature([SharedEffects])
  ],
  exports: [],
  providers: [],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return { ngModule: SharedModule };
  }
}
