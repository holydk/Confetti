import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { SharedModule } from '@shared/shared.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UniversalStorage } from '@shared/storage/universal.storage';

import { TransferHttpCacheModule } from '@nguniversal/common';
import { CookieService, CookieModule } from 'ngx-cookie';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'app' }),
    TransferHttpCacheModule,
    HttpClientModule,
    AppRoutingModule,
    CookieModule.forRoot(),
    SharedModule.forRoot()
  ],
  providers: [
    CookieService,
    UniversalStorage,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
