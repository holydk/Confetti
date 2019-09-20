import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { IdentityEffects } from './state/effects/identity.effects';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    EffectsModule.forFeature([IdentityEffects])
  ]
})
export class IdentityModule { }
