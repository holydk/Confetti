import { NgModule } from '@angular/core';
import { ServerModule, ServerTransferStateModule } from '@angular/platform-server';

import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';

import { AppComponent } from './app.component';
import { AppModule } from './app.module';
import { CookieService, CookieBackendService } from 'ngx-cookie';

@NgModule({
  imports: [
      // AppModule - FIRST!!!
      AppModule,
      ServerModule,
      ServerTransferStateModule,
      ModuleMapLoaderModule
    ],
  bootstrap: [AppComponent],
  providers: [{
      provide: CookieService,
      useClass: CookieBackendService
    }]
  })
export class AppServerModule {}
