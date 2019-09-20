import { NgModule } from '@angular/core';
import { BrowserTransferStateModule } from '@angular/platform-browser';

import { REQUEST } from '@nguniversal/express-engine/tokens';

import { AppComponent } from './app.component';
import { AppModule } from './app.module';

// the Request object only lives on the server
export function getRequest(): any {
  return { headers: { cookie: document.cookie } };
}

@NgModule({
  bootstrap: [AppComponent],
  imports: [
    AppModule,
    BrowserTransferStateModule
  ],
  providers: [
    {
    // The server provides these in main.server
      provide: REQUEST,
      useFactory: getRequest,
    },
    {
      provide: 'ORIGIN_URL',
      useValue: location.origin
    }
  ]
})
export class AppBrowserModule {}
