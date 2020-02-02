import { Component, ViewChild, EventEmitter, OnDestroy, enableProdMode, Input, Output, OnInit, ElementRef, AfterViewInit} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { Globals } from '../../../shared';
import { FileService } from '../../services';
import { Observable, fromEvent } from 'rxjs';
import { take } from 'rxjs/operators';

import { Element } from '@angular/compiler/src/render3/r3_ast';

import { ConfirmEditFormComponent } from '../confirm-edit-form/confirm-edit-form.component';

@Component({
  selector: "file-item",
  templateUrl: './file-item.component.html',
  styleUrls: ['./file-item.component.css']
})
export class FileItemComponent implements OnInit, OnDestroy{

  @Input() file: any;
  @Input() infoform: any;
  @Input() confirm: any;
  
  @Input() isReadOnly: boolean = true;
  @Input() isInformation: boolean = false;
  @Output() onRefresh: EventEmitter<any> = new EventEmitter<any>();
  @Output() onCheckChanged: EventEmitter<any> = new EventEmitter<any>();

  clickEvent: any;
  isChecked: boolean = false;

  @ViewChild("confirmitem") confirmitem: ConfirmEditFormComponent;
  

  constructor(private fileService: FileService) {
    
  }

  ngOnInit() {

  }

  Checked() {
    this.isChecked = !this.isChecked;
    if (this.onCheckChanged)
      this.onCheckChanged.emit();
  }

  ngOnDestroy() {   
    if (this.clickEvent) {
      this.clickEvent = null;
    }
  }

  downloadFile(id) {
    this.fileService.getFile(id).subscribe(
      data => {
        Globals.dowloadBlobData(data, 'data.dat');
      },
      error => {
        console.log("Ошибка скачивания файла");
        this.infoform.showPopup('Ошибка', 'Ошибка загрузки файла');
      });
  }

  deleteRecord(id: number) {
    this.fileService.deleteFile(id).subscribe(
      data => {
        if (this.onRefresh)
          this.onRefresh.emit();
      },
      error => {
        this.infoform.showPopup('Ошибка', 'Ошибка удаления файла');
      });

  }

  deleteElementFile(e): any {
    e.preventDefault();
    e.stopPropagation();
    this.isChecked = false;
    if (this.onCheckChanged)
      this.onCheckChanged.emit();
    this.confirmitem.showPopup('Удаление файла', 'Удалить файл?', () => {
      this.deleteRecord(e.target.getAttribute("data-fileid"));
    })
  }
 

}
