import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import {
  SharedModule as AdminSharedModule
} from '@admin/shared/shared.module';
import { AdminRoutes } from './admin.routing';
import { AdminComponent } from './admin.component';

@NgModule({
  declarations: [AdminComponent],
  imports: [
    CommonModule,
    AdminRoutes,
    RouterModule,
    AdminSharedModule.forRoot()
  ]
})
export class AdminModule { }
