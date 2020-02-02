import { Component, ViewChild, EventEmitter, OnDestroy, enableProdMode} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';


import {
  DxPopupComponent
} from 'devextreme-angular';

@Component({
  selector: "info-form",
  templateUrl: './info-form.component.html',
  styleUrls: ['./info-form.component.css']
})
export class InfoFormComponent implements OnDestroy {

  @ViewChild(DxPopupComponent) confirm_popup: DxPopupComponent;
  onCancel: EventEmitter<any> = new EventEmitter();
  infotext: string;
  
  popupVisible: boolean = false;

  constructor() {
    this.popupVisible = false;
  }

  closePopup() {
    this.popupVisible = false;
    if (this.onCancel)
      this.onCancel.emit();
  }



  showPopup(title: string, info: string, onCancel: any = null) {
    this.confirm_popup.title = title;
    this.confirm_popup.showTitle = true;
    if (onCancel) {
      this.onCancel.subscribe(onCancel);
    }
    
    this.infotext = info;
    this.popupVisible = true;
  }

  ngOnDestroy() {
    if (this.onCancel)
      this.onCancel.unsubscribe();
  }
 

}
