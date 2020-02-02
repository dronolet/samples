import { NgModule, APP_INITIALIZER } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule, ConfigService } from './core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { locale, loadMessages } from 'devextreme/localization';
import 'devextreme-intl';
import * as messagesRu from "devextreme/localization/messages/ru.json";


// components
import { AppComponent } from './app.component';


export function init(_boot: ConfigService) {
  return () => {   
    return _boot.getConfig();
  };
}

loadMessages(messagesRu);
locale("ru");

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    //BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    CoreModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: init,
      deps: [ConfigService],
      multi: true
    }
  ], 
  bootstrap: [AppComponent]
})
export class AppModule { }
