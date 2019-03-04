import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NotFoundComponent } from './pages/not-found.component';
import { NotFoundService } from './not-found.service';
import { NotFoundRoutes } from './not-found.routing';

@NgModule({
  imports: [
    CommonModule,
    NotFoundRoutes
  ],
  providers: [NotFoundService],
  declarations: [NotFoundComponent],
})
export class NotFoundModule {}
