import { Component, OnInit, Input, Output, ViewChildren, ViewChild, EventEmitter, QueryList } from '@angular/core';
import { Globals } from '../../../shared';
import { FileService } from '../../services';
import { Observable } from 'rxjs';
import { ConfirmEditFormComponent } from '../confirm-edit-form/confirm-edit-form.component';
import { InfoFormComponent } from '../info-form/info-form.component';
import { FileItemComponent } from '../file-item/file-item.component';

@Component({
  selector: 'show-files',
  templateUrl: './show.files.component.html',
  styleUrls: ['./show.files.component.css']
})
export class ShowFilesComponent implements OnInit {

  @Input() isReadOnly: boolean = false;
  @Input() isInformation: boolean = false;
  @Input() orderid: number = null;

  fileList: Observable<Array<any>>;
  tinfoform: any;
  tconfirm: any;
  isShowSend: boolean = false;
  InformationMode: boolean = false;


  @ViewChild("confirm") confirm: ConfirmEditFormComponent;
  @ViewChild("infoform") infoform: InfoFormComponent;
  @ViewChildren(FileItemComponent) fileItems: QueryList<FileItemComponent>

  constructor(private fileService: FileService) {

  }


  ngOnInit() {
    this.refresfFiles();
    this.tinfoform = this.infoform;
    this.tconfirm = this.confirm;
    this.InformationMode = this.isInformation;
  }

  refresfFiles() {
    this.fileList = null;
    if (this.orderid && this.orderid > 0)
      this.fileService.getFiles(this.orderid).subscribe(
        data => this.fileList = data
      )
  }

  onCheckChanged() {
    this.isShowSend = this.fileItems.filter(file => file.isChecked == true).length > 0;
  }

  SendInstruction() {
    var idsList: any = this.fileItems.filter(file => file.isChecked == true).map(o => o.file.id);
    this.confirm.showPopup('Предписание', 'Отправить предписание?', () => {
      this.fileService.sendFiles({ orderid: this.orderid, ids: idsList }).toPromise().then(
        data => this.infoform.showPopup('Информация', 'Успешная отправка')
      ).catch(error =>
        this.infoform.showPopup('Ошибка', 'Ошибка операции')
      );
    });
  }

  handleFileInput(files: FileList) {
    let file = files.item(0);
    let formData = new FormData();
    formData.append('fileName', file, file.name);
    this.fileService.addFile(this.orderid, formData).toPromise()
        .then(
          data => this.refresfFiles()
        )
       .catch(
          error => this.infoform.showPopup('Ошибка', 'Ошибка добвления')
      ); 
  }

}
