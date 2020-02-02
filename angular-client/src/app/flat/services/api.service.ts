import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from '../../core';


@Injectable()
export class ApiService {

  constructor(private http: HttpClient, private configService: ConfigService) { } //

  getRecord(id: any): any {
    return this.http.get<any>(this.configService.configData.baseUrl + '/data/orders/' + id + '/?r=' + Math.random());
  }

  updateRecord(data: any): any {
    if (data.id)
      return this.http.put<any>(this.configService.configData.baseUrl + '/data/orders/' + data.id + '/?r=' + Math.random(), data);
    else return this.http.put<any>(this.configService.configData.baseUrl + '/data/orders/?r=' + Math.random(), data);
  }

  getObjects(): any {
    return this.http.get<any>(this.configService.configData.baseUrl + '/data/objects/?r=' + Math.random());
  }

  getContragents(path): any {
    return this.http.get<any>(this.configService.configData.baseUrl + '/data/contragents/' + encodeURI(path) + '/?r=' + Math.random());
  }
  
  

  deleteRecord(id: any): any {
    return this.http.delete<any>(this.configService.configData.baseUrl + '/data/orders/' + id + '/?r=' + Math.random()).toPromise()
      .then((data: any) => {
        return data;
      })
      .catch(e => {
        alert('Ошибка удаления');
        //throw e && e.error && e.error.Message;
      });
  }

  getHeads(): any {
    return this.http.get<any>(this.configService.configData.baseUrl + '/data/heads/?r=' + Math.random());
  }

  getEmployees(): any {
    return this.http.get<any>(this.configService.configData.baseUrl + '/auth/employees/?r=' + Math.random());
  }
 
  getData(filter): any { 
    return this.http.post<any>(this.configService.configData.baseUrl + '/data/orders/?r=' + Math.random(), filter).toPromise()
      .then((data: any) => {
        return data;
      })
      .catch(e => {
        throw e && e.error && e.error.Message;
      });
  }

}

