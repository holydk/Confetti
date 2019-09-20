import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';

import { UserComponent } from './user.component';
import { UserRoutes } from './user.routing';
import { LayoutsModule } from './layouts/layouts.module';
import { userStateKey, userReducers } from './state/reducers/user.reducer';

@NgModule({
  declarations: [UserComponent],
  imports: [
    CommonModule,
    UserRoutes,
    RouterModule,
    LayoutsModule,
    StoreModule.forFeature(userStateKey, userReducers)
  ]
})
export class UserModule { }
