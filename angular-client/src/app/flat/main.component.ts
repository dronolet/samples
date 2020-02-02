import { Component, OnInit, ViewChild, AfterViewInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DxDataGridComponent } from 'devextreme-angular';
import { ApiService, ReportsService } from './services';
import { Filter } from './models/filter';
import { CommonFunctions, Globals } from '../shared';

import CustomStore from 'devextreme/data/custom_store';
import {
  EditFormComponent,
  DatePeriodComponent,
  DateEmploeePeriodComponent,
  InfoFormComponent
} from './components'

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit, AfterViewInit {

  dataSource: CustomStore;
  flats: any;
  previewFlat: any;
  filter: Filter;
  pageIndex: number = 0;
  userinfo: any;
  editVisible: boolean = false;
  elheader: any;

  @ViewChild('grid') grid: DxDataGridComponent;
  @ViewChild('editform') editform: EditFormComponent;
  @ViewChild('dateperiod') dateperiod: DatePeriodComponent;
  @ViewChild('dateemploee') dateEmoloeePeriod: DateEmploeePeriodComponent
  @ViewChild('infoform') infoform: InfoFormComponent;
  
  
  reportsList: { id: number, name: string }[] = [
    { id: 1, name: 'Отчет по сотрудникам' },
    { id: 2, name: 'Отчет по подрядчикам' },
    { id: 3, name: 'Отчет по сотруднику' },
    { id: 4, name: 'Отчет по объектам' }
  ];

  getShortName(name: string) {
    let nameParts = name.split(" ");
    return nameParts[0] + (nameParts.length > 1 ? ' ' + nameParts[1].charAt(0) + '.' : '') + (nameParts.length > 2 ? ' ' +nameParts[2].charAt(0) + '.' : '');
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private apiService: ApiService,
    private reportService: ReportsService

  ) {
    this.userinfo = this.route.snapshot.data['userinfo']
    if (!this.userinfo) {
      this.router.navigate(['/login']);
      return;
    }
    this.filter = Filter.getNew();
    this.userinfo["shrtName"] = this.getShortName(this.userinfo["fullName"]);
    this.previewClick = this.previewClick.bind(this);
    this.dataSource = new CustomStore({
      key: "id",
      load: () => this.apiService.getData({
          dfrom: (this.filter.from?this.filter.from.toUTCString():null),
          dto: (this.filter.to?this.filter.to.toUTCString():null),
          overdue: (this.filter.isOverdue ? 1 : 0)
      }),
      remove: (key) => this.apiService.deleteRecord(key)
    });
  }

  setisOverdue() {
    this.filter.isOverdue = !this.filter.isOverdue;    
  }

  doSearch() {
    this.grid.instance.refresh();
  }
  
  onOptionChanged(e) {
    this.filter.isOverdue = false;
  }

  OnApplyDatePeriod(e) {
    this.reportService.getEmploeeReport(e).toPromise().then(
      data => {
        CommonFunctions.dowloadBlobData(data, 'file.dat');
      }
    ).catch(
      err => this.infoform.showPopup('Ошибка', 'Ошибка операции')
    );   
  }

  OnApplyemploeeDatePeriod(e) {
    this.reportService.getEmploeeReportDate(e).toPromise().then(
      data => {
        CommonFunctions.dowloadBlobData(data, 'file.dat');
      }
    ).catch(
      err => this.infoform.showPopup('Ошибка', 'Ошибка операции')
    );   
  }

  getReport() {
    this.reportService.getDataReport({
      dfrom: (this.filter.from ? Globals.toUTC(this.filter.from) : null),
      dto: (this.filter.to ? Globals.toUTC(this.filter.to) : null),
      overdue: (this.filter.isOverdue ? 1 : 0)
    }).toPromise().then(
      data => {
        CommonFunctions.dowloadBlobData(data, 'file.dat');
      }
    ).catch(
      err => this.infoform.showPopup('Ошибка', 'Ошибка операции')
    );   
  }

  OnReport(reportNumber: number) {    
    if ([1, 2, 4].indexOf(reportNumber) > -1) {
      this.dateperiod.showPopup(this.reportsList[reportNumber - 1].name, reportNumber);
    } else this.dateEmoloeePeriod.showPopup(this.reportsList[reportNumber - 1].name,
      (status: boolean) => {
        if (!status)
         this.infoform.showPopup('Ошибка', 'Ошибка операции');
      }
    );
  }

  logOut() {
    this.router.navigate(['/login']);
  }


  ngOnInit() {
    if (localStorage.getItem('pageIndex')) {
      try {
        this.pageIndex = Number.parseInt(localStorage.getItem('pageIndex'));
      } catch {
        this.pageIndex = 0;
      }
    }
  }

  ngAfterViewInit() {
    
  }

  @HostListener('window:scroll', ['$event']) onWindowScroll(e) {
    
    let scroll: number = window.pageYOffset || document.documentElement.scrollTop;

    if (scroll > 150) {
      
      if (!this.elheader) {
        this.elheader = document.querySelector(".dx-datagrid-headers");        
        if (this.elheader) {
          this.elheader.classList.add("header-top");
        }
      }
    } else {
      if (this.elheader)
        this.elheader.classList.remove("header-top");

      this.elheader = null;
    }
  }

  refreshData() {
   this.grid.instance.refresh();
  }

  previewClick(e) {
    this.showPreviewClick(e.row.data);
  }

  addNew() {
    this.editform.show(null);
  }

  showPreviewClick(data: any) {
    //this.flatinfo.currentFlat = data;
    //this.flatinfo.showForm();
  }

  onRowDblClick(e) {
    if (this.grid.instance.getSelectedRowsData().length > 0)
      this.showRecord(this.grid.instance.getSelectedRowsData()[0].id);
  }

  showRecord(id: number) {
    localStorage.setItem('pageIndex', this.grid.instance.pageIndex().toString());
    this.editform.show(id);
  }

  editClick(e) {
    this.showRecord(e.data.id);
  }

  deleteClick(e) {
    this.grid.instance.deleteRow(e.rowIndex);
  }

}
