import { Component, ViewChild, EventEmitter, OnDestroy, enableProdMode, Output} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DateFilter } from '../../../models';
import { ReportsService } from '../../../services';
import { Globals } from '../../../../shared/classes/globals';

import {
  DxPopupComponent
} from 'devextreme-angular';

@Component({
  selector: "date-period",
  templateUrl: './dateperiod.component.html',
  styleUrls: ['./dateperiod.component.css']
})
export class DatePeriodComponent implements OnDestroy {

  @ViewChild(DxPopupComponent) dialogpopup: DxPopupComponent;
  @Output() OnApply: EventEmitter<any> = new EventEmitter<any>(); 
  
  filter: DateFilter = DateFilter.getNew();
  filterReportId: number;
  popupVisible: boolean = false;

  constructor() {
    this.popupVisible = false;
  }

  

  DatePerpare() {
    let dfilter: DateFilter = { ... this.filter };
    dfilter.dfrom = Globals.toUTC(dfilter.dfrom);
    dfilter.dto = Globals.toUTC(dfilter.dto);
    return dfilter;
  }

  closePopup() {
    this.popupVisible = false;
    let saveFilter: DateFilter = this.DatePerpare();
    if (this.OnApply)
      this.OnApply.emit({ ...saveFilter, reportId: this.filterReportId});
  }

  showPopup(title: string, report: number) {
    this.dialogpopup.title = title;
    this.dialogpopup.showTitle = true;
    this.filterReportId = report;

    this.popupVisible = true;
  }

  ngOnDestroy() {

  }
 

}
