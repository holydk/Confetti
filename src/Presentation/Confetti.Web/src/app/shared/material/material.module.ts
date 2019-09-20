import { NgModule } from '@angular/core';
import {
  MatToolbarModule,
  MatIconModule,
  MatMenuModule,
  MatButtonModule,
  MatDividerModule,
  MatGridListModule,
  MatChipsModule,
  MatSelectModule
} from '@angular/material';

const MODULES = [
  MatToolbarModule,
  MatIconModule,
  MatMenuModule,
  MatButtonModule,
  MatDividerModule,
  MatGridListModule,
  MatChipsModule,
  MatSelectModule
];

@NgModule({
  exports: MODULES,
  imports: MODULES
})
export class MaterialModule { }
