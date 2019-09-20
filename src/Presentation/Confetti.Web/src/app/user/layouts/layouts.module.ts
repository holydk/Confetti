import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';

import { FooterComponent } from './footer/components/footer.component';
import { ToolbarComponent } from './toolbar/components/toolbar.component';
import { WrapperComponent } from './wrapper/components/wrapper.component';
import { HeaderComponent } from './header/header.component';
import { LayoutsEffects } from './state/effects/layouts.effects';
import { MaterialModule } from '@shared/material/material.module';
import { AccountDropdownComponent } from './header/components/account-dropdown/account-dropdown.component';
import { CartDropdownComponent } from './header/components/cart-dropdown/cart-dropdown.component';
import { RootCategoriesComponent } from './header/components/root-categories/root-categories.component';
import { CategoriesDropdownComponent } from './header/components/categories-dropdown/categories-dropdown.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    EffectsModule.forFeature([LayoutsEffects]),
    MaterialModule
  ],
  declarations: [
    FooterComponent,
    ToolbarComponent,
    WrapperComponent,
    HeaderComponent,
    AccountDropdownComponent,
    CartDropdownComponent,
    RootCategoriesComponent,
    CategoriesDropdownComponent
  ]
})
export class LayoutsModule {}
