import { Component, OnInit, Input, Output, ViewChild, EventEmitter, OnDestroy } from '@angular/core';
import { DxValidationGroupComponent, DxSelectBoxComponent } from 'devextreme-angular';
import { Order } from '../../models';
import { Globals } from '../../../shared';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../../core';
import { InfoFormComponent } from 'src/app/flat/components/info-form/info-form.component';
import CustomStore from 'devextreme/data/custom_store';
import { forkJoin } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import DataSource from 'devextreme/data/data_source';



@Component({
  selector: 'edit-form-main',
  templateUrl: './edit.form.component.html',
  styleUrls: ['./edit.form.component.css']
})
export class EditFormComponent implements OnInit, OnDestroy {

  @Output() onChanged: EventEmitter<any> = new EventEmitter<any>(); 
  popupVisible: boolean;
  title: string;
  order: Order = null;
  isNew: boolean = false;
  isDateFact: boolean = false;
  isNuberRemark: boolean;
  isSavable: boolean = true;
  isNoRemark: boolean = false;
  isInited: boolean = false;
  arLocations: any;
  heads: any;
  headsRecords: any;
  arOnorUsers: any;
  
  contragents: any;
  pathsubscr$: any;
 
  dbegin: any;
  isVisibleHeads: boolean = false;
  tabPanelItems: { id: number, title: string }[] = [];

  headsDataSource: DataSource;

  arResults: { id: number, name: string }[] = [
    { id: 1, name: 'Без замечания' },
    { id: 2, name: 'Замечание' },
    { id: 3, name: 'Предписание' }
  ];

  @ViewChild("validationGroupVar") validationGroup: DxValidationGroupComponent;
  @ViewChild("infoform") infoform: InfoFormComponent;
  @ViewChild("contragentsList") contragentsList: DxSelectBoxComponent;
  @ViewChild("heads") headsList: DxSelectBoxComponent;
  

  constructor(private apiService: ApiService, private authService: AuthService) {}

  ngOnInit() {
    this.isNoRemark = false;
    this.popupVisible = false;
    this.order = null;
  }

  valuecghngedS(e) {
    this.contragents = null;
    if (this.pathsubscr$) {
      this.pathsubscr$.unsubscribe();
      this.pathsubscr$ = null;
    }
    if (this.contragentsList.text && this.contragentsList.text.length > 2) {
      this.pathsubscr$ = this.apiService.getContragents(this.contragentsList.text).subscribe(
        data => {
          this.contragents = data;
        },
        error => { }
      );
    }
    this.contragentsList.instance.close();
  }


 
  show(id: number) {
    this.popupVisible = false;
    this.isSavable = true;
    this.isNuberRemark = false;
    this.isInited = false;
    this.isDateFact = false;
    this.order = null;
    this.isVisibleHeads = false;
    this.isNew = (id == null ? true : false);
    let thisClass: any = this;

    

    this.tabPanelItems = [{
      "id": 0,
      "title": "Общие"
    },
    {
      "id": 1,
      "title": "Файлы"
    }
    ];

    if (this.isNew) {
      this.tabPanelItems.pop();
      this.title = 'Новая запись';
      this.order = Order.NewOrder();
      forkJoin(
        this.apiService.getObjects(),
        this.apiService.getHeads()
      ).subscribe(
        data => {
          this.popupVisible = true;
          this.arLocations = data[0];
         
          this.popupVisible = (this.arLocations && this.arLocations.length);
          
        },
        error => {
          this.infoform.showPopup('Ошибка', 'Ошибка открытия.');
          this.popupVisible = false;

        }
      );
    }
    else {
      this.title = 'Редактировать';
      this.popupVisible = true;
      this.apiService.getRecord(id).pipe(
        switchMap((data:any) => {
          thisClass.order = Order.parseOrder(data);         
          return forkJoin(
            this.apiService.getObjects(),
            this.apiService.getHeads()
          )
        })
      ).subscribe(
        data => {
          this.arLocations = data[0];
          this.headsRecords = data[1].result;
          if (this.order != null) {
            this.isNoRemark = this.order.result == 1;
            this.isSavable = (this.order.canEdit == 1);
            this.isDateFact = (this.order.repareFakt != null);
            this.isInited = true;
            this.popupVisible = true;
          }
          else {
            
            this.infoform.showPopup('Ошибка', 'Ошибка открытия.');
            this.popupVisible = false;
          }
        },
        error => {
          this.infoform.showPopup('Ошибка', 'Ошибка открытия.');
          this.popupVisible = false;
        }
      );
    }
  }


  valuecghnged(e) {
    if (this.arResults) {
      var item = e.component.option('selectedItem');
      if (item) {
            this.isNuberRemark = (item.id == 3);
            this.isNoRemark = item.id == 1;
            if (this.isInited) 
              this.order.remark = '';
            if (this.isNoRemark)
             this.order.repareFakt = null;                
        }
      
    }
  }

  

  saveData() {
    
    if (this.validationGroup.instance.validate().isValid) {

      let currDate = new Date();
      
     
      if (this.order.dBegin >= this.order.dEnd) {
        this.infoform.showPopup('Ошибка сохранения', 'Неверный диапозон времени.');
        return;
      }

      if (this.order.dBegin >= this.order.dEnd) {
        this.infoform.showPopup('Ошибка сохранения','Неверный диапозон времени.');
        return;
      }

      if (!this.order.buildObject) {
         this.infoform.showPopup('Ошибка сохранения', 'Укажите объект.');
         return;
      }

      if (!this.order.location) {
        this.infoform.showPopup('Ошибка сохранения', 'Укажите "Место".');
        return;
      }

      if (!this.order.workType) {
        this.infoform.showPopup('Ошибка сохранения', 'Укажите "Виды работ".');
        return;
      }

      if (!this.order.contractor) {
        this.infoform.showPopup('Ошибка сохранения', 'Укажите "Подрядчика".');
        return;
      }

     
        
      if (!this.order.repare) {
        this.infoform.showPopup('Ошибка сохранения', 'Не указана дата "Дата устранения"');
        return;
      }


      if (this.isNew && Globals.DateWithotTime(this.order.repare) < Globals.DateWithotTime(currDate)) {
        this.infoform.showPopup('Ошибка сохранения', 'Дата устранения меньше текущей даты.');
        return;
      }

      if (Globals.DateWithotTime(this.order.repare) < Globals.DateWithotTime(this.order.dBegin)) {
        this.infoform.showPopup('Ошибка сохранения', '"Дата устранения" не может быть меньше даты создания замечания.');
        return;
      }

      
      if (this.order.result == 2 && Globals.DateWithotTime(this.order.repare) > Globals.addDays(Globals.DateWithotTime(this.order.dBegin), 3)) {
        this.infoform.showPopup('Ошибка сохранения', '"Дата устранения" не может быть > 4 дней от даты создания замечания.');
        return;
      }

      
      if (this.order.result == 3 && Globals.DateWithotTime(this.order.repare) > Globals.addMonth(Globals.DateWithotTime(this.order.dBegin), 1)) {
        this.infoform.showPopup('Ошибка сохранения', '"Дата устранения" не может быть > месяца от даты создания замечания.');
        return;
      }


      if (this.order.repareFakt && Globals.DateWithotTime(this.order.repareFakt) > Globals.DateWithotTime(new Date())) {
        this.infoform.showPopup('Ошибка сохранения', 'Дата факт. длжна быть <= текущей даты.');
        return;
      }

     
      
      if (this.order.result == 2) {
        if (!this.order.remark || (this.order.remark.toString().trim().length == 0) ) {
          this.infoform.showPopup('Ошибка сохранения', 'Укажите примечание.');
          return;
        }
      }
      
      let orderSave: any = {...this.order };
      orderSave.dBegin = orderSave.dBegin.toUTCString(); 
      orderSave.dEnd = orderSave.dEnd.toUTCString();
      orderSave.repare = orderSave.repare.toUTCString();
      if (orderSave.repareFakt)
        orderSave.repareFakt = orderSave.repareFakt.toUTCString();

      this.apiService.updateRecord(orderSave).toPromise()
        .then(
        data => {

          if (data && data.error) 
            this.infoform.showPopup('Ошибка', data.error);

          this.popupVisible = false;
           if (this.onChanged)
              this.onChanged.emit();
        }
      )
      .catch(
        error => {
          this.infoform.showPopup('Ошибка', 'Ошибка сохранения.');
        }
      )

    }

  }

  ngOnDestroy() {
    if (this.pathsubscr$)
      this.pathsubscr$.unsubscribe();
  }

}
