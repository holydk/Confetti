import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AboutComponent } from './pages/about/about.component';
import { AboutRoutes } from './about.routing';

@NgModule({
  declarations: [AboutComponent],
  imports: [
    CommonModule,
    AboutRoutes
  ]
})
export class AboutModule { }
