import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { FooterComponent } from './footer/components/footer.component';
import { ToolbarComponent } from './toolbar/components/toolbar.component';
import { WrapperComponent } from './wrapper/components/wrapper.component';

@NgModule({
  imports: [
      CommonModule,
      RouterModule
    ],
  declarations: [
      FooterComponent,
      ToolbarComponent,
      WrapperComponent
    ]
})
export class LayoutsModule {}
