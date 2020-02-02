import { Component, ViewChild, EventEmitter, OnDestroy, enableProdMode} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  DxPopupComponent
} from 'devextreme-angular';

@Component({
  selector: "confirm-edit-form",
  templateUrl: './confirm-edit-form.component.html',
  styleUrls: ['./confirm-edit-form.component.css']
})
export class ConfirmEditFormComponent implements OnDestroy {

  @ViewChild(DxPopupComponent) confirm_popup: DxPopupComponent;
  onConfirm: EventEmitter<any>;
  onCancel: EventEmitter<any>;
  infotext: string;
  
  popupVisible: boolean = false;

  constructor() {
    this.popupVisible = false;
  }

  closePopup() {    
    this.popupVisible = false;
    if (this.onCancel) {
      this.onCancel.emit();
      this.onConfirm.unsubscribe();
    }      
  }

  yesAction() {

    this.popupVisible = false;
    
    if (this.onConfirm) {
      setTimeout(() => {
        this.onConfirm.emit();
        if (this.onConfirm)
          this.onConfirm.unsubscribe();
      }, 2);      
    }
  }

  showPopup(title: string, info: string, onConfirm: any, onCancel: any = null) {
    this.confirm_popup.title = title;
    this.confirm_popup.showTitle = true;
    
    if (onCancel) {
      this.onCancel = new EventEmitter<any>();
      this.onCancel.subscribe(onCancel);
    }
    this.infotext = info;
    if (onConfirm) {
      this.onConfirm = new EventEmitter<any>();
      this.onConfirm.subscribe(onConfirm);
    }
    this.popupVisible = true;
  }

  unsubscribe() {
    try {
      if (this.onConfirm)
        this.onConfirm.unsubscribe();
      if (this.onCancel)
        this.onCancel.unsubscribe();
    } catch {
    }
  }

  ngOnDestroy() {
    
    this.unsubscribe();
  }
 

}
