import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest } from '@angular/common/http';
import { ConfigService } from '../../core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable()
export class ReportsService {

  constructor(private http: HttpClient, private configService: ConfigService) { } //

  getEmploeeReport(data: any): any {
    return this.http.post(this.configService.configData.baseUrl + '/reports/preport/?r=' + Math.random(), data, { observe: 'response', responseType: 'blob' });
  }

  getEmploeeReportDate(data: any): any {
    return this.http.post(this.configService.configData.baseUrl + '/reports/emploee/?r=' + Math.random(), data, { observe: 'response', responseType: 'blob' });
  }

  getDataReport(data: any): any {
    return this.http.post(this.configService.configData.baseUrl + '/reports/orders/?r=' + Math.random(), data, { observe: 'response', responseType: 'blob' });
  }

}

