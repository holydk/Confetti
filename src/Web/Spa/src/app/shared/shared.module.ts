import { ModuleWithProviders, NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { SharedEffects } from './state/effects/shared.effects';
import { MaterialModule } from './material/material.module';
import { NguCarouselModule } from '@ngu/carousel';

@NgModule({
  imports: [
    EffectsModule.forFeature([SharedEffects]),
    MaterialModule,
    NguCarouselModule
  ],
  exports: [
    MaterialModule,
    NguCarouselModule
  ],
  providers: [],
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return { ngModule: SharedModule };
  }
}
