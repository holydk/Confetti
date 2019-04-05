import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';

import { FooterComponent } from './footer/components/footer.component';
import { ToolbarComponent } from './toolbar/components/toolbar.component';
import { WrapperComponent } from './wrapper/components/wrapper.component';
import { HeaderComponent } from './header/header.component';
import { TopMenuComponent } from './header/components/top-menu/top-menu.component';
import { MenuComponent } from './header/components/menu/menu.component';
import { LayoutsEffects } from './state/effects/layouts.effects';

@NgModule({
  imports: [
      CommonModule,
      RouterModule,
      EffectsModule.forFeature([LayoutsEffects])
    ],
  declarations: [
      FooterComponent,
      ToolbarComponent,
      WrapperComponent,
      HeaderComponent,
      TopMenuComponent,
      MenuComponent
    ]
})
export class LayoutsModule {}
