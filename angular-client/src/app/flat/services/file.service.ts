import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from '../../core';



@Injectable()
export class FileService {

  private headers: HttpHeaders = new HttpHeaders();

  constructor(private http: HttpClient, private configService: ConfigService) { } //

  getFiles(orderid: number): any {
    return this.http.get(this.configService.configData.baseUrl + '/file/list/' + orderid.toString() + '/?r=' + Math.random());
  }

  sendFiles(data: any): any {
    return this.http.post(this.configService.configData.baseUrl + '/file/sendmail/?r=' + Math.random(), data);
  }

  addFile(orderid: number, data: any): any {
    
    this.headers.append('Accept', 'application/json');
    this.headers.append('Content-Type', 'multipart/form-data');  
    return this.http.post(this.configService.configData.baseUrl + '/file/'+ orderid.toString() +'/?r=' + Math.random(), data, { headers: this.headers });
  }

  deleteFile(id: number): any {
    return this.http.delete(this.configService.configData.baseUrl + '/file/' + id.toString() + '/?r=' + Math.random());
  }

  getFile(id: number): any {
    return this.http.get(this.configService.configData.baseUrl + '/file/' + id.toString() + '/?r=' + Math.random(), { observe: 'response', responseType: 'blob' });
  }
  

}

