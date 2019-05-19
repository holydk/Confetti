import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { EffectsModule } from '@ngrx/effects';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { CookieService, CookieModule } from 'ngx-cookie';
import { StoreModule } from '@ngrx/store';
import { MetaModule } from '@ngx-meta/core';

import { SharedModule } from '@shared/shared.module';
import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { UniversalStorage } from '@shared/storage/universal.storage';
import { appReducers } from './state/reducers/app.reducer';
import { CoreModule } from './core/core.module';

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
    StoreModule.forRoot(appReducers),
    CookieModule.forRoot(),
    SharedModule.forRoot(),
    EffectsModule.forRoot([]),
    MetaModule.forRoot(),
    CoreModule.forRoot()
  ],
  providers: [
    CookieService,
    UniversalStorage,
  ]
})
export class AppModule {}
