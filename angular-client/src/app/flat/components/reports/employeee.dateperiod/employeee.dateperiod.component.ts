import { Component, ViewChild, EventEmitter, OnDestroy, enableProdMode, Output} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { DateFilter } from '../../../models';
import { ApiService } from '../../../services/api.service';
import { Globals } from '../../../../shared/classes/globals';

import {
  DxPopupComponent
} from 'devextreme-angular';

@Component({
  selector: "employeee-dateperiod",
  templateUrl: './employeee.dateperiod.component.html',
  styleUrls: ['./employeee.dateperiod.component.css']
})
export class DateEmploeePeriodComponent implements OnDestroy {

  @ViewChild(DxPopupComponent) dialogpopup: DxPopupComponent;
  @Output() OnApply: EventEmitter<any> = new EventEmitter<any>();

  filter: DateFilter = DateFilter.getNew();
  employee: number;

  EmployersList: any;
  popupVisible: boolean = false;
  

  constructor(private apiService: ApiService) {
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
      this.OnApply.emit({ ...saveFilter, emploeeId: this.employee });
  }



  showPopup(title: string, result: (status: boolean) => void){
    this.dialogpopup.title = title;
    this.dialogpopup.showTitle = true;
    this.apiService.getEmployees().toPromise().then(
      data => {
        this.EmployersList = data;
        this.popupVisible = true;
      }
    ).catch(
      error => {
        result(false);
      }
    );
  }

  ngOnDestroy() {
  }
 

}
