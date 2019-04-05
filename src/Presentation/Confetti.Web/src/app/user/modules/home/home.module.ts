import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { HomeComponent } from './pages/home.component';
import { HomeRoutes } from './home.routing';
import { homeReducer, homeStateKey } from './state/reducers/home.reducer';
import { HomeEffects } from './state/effects/home.effects';
import { HomeService } from './home.service';

@NgModule({
  imports: [
    CommonModule,
    HomeRoutes,
    StoreModule.forFeature(homeStateKey, homeReducer),
    EffectsModule.forFeature([HomeEffects])
  ],
  declarations: [HomeComponent],
  providers: [
    HomeService
  ]
})
export class HomeModule {}
