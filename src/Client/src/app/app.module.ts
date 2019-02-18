import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { SharedModule } from '@shared/shared.module';
import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { UniversalStorage } from '@shared/storage/universal.storage';

import { TransferHttpCacheModule } from '@nguniversal/common';
import { CookieService, CookieModule } from 'ngx-cookie';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'my-app' }),
    TransferHttpCacheModule,
    HttpClientModule,
    RouterModule,
    AppRoutes,
    CookieModule.forRoot(),
    SharedModule.forRoot()
  ],
  providers: [
    CookieService,
    UniversalStorage,
  ]
})
export class AppModule {}
