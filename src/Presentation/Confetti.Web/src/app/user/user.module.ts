import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import {
  SharedModule as UserSharedModule
} from '@user/shared/shared.module';
import { UserComponent } from './user.component';
import { UserRoutes } from './user.routing';

@NgModule({
  declarations: [UserComponent],
  imports: [
    CommonModule,
    UserRoutes,
    RouterModule,
    UserSharedModule.forRoot()
  ]
})
export class UserModule { }
