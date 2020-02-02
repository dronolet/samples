import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { FlatRoutingModule } from './flat-routing.module';
import {
  ApiService,
  FileService,
  ReportsService
} from './services';
import { SharedModule } from '../shared/shared.module';
import {
  EditFormComponent,
  ShowFilesComponent,
  ConfirmEditFormComponent,
  InfoFormComponent,
  FileItemComponent,
  DatePeriodComponent,
  DateEmploeePeriodComponent
} from './components';




@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    HttpClientModule,
    FormsModule,
    FlatRoutingModule,
    
  ],
  declarations: [
    MainComponent,
    ShowFilesComponent,
    EditFormComponent,
    FileItemComponent,
    ConfirmEditFormComponent,
    InfoFormComponent,
    DatePeriodComponent,
    DateEmploeePeriodComponent
  ],
  providers: [
    ApiService,
    FileService,
    ReportsService
  ]
})
export class FlatModuleNgFactory {}



