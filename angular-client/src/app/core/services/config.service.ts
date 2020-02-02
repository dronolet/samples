import { Injectable } from '@angular/core';
import { Location} from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import { CoreModule } from '../core.module';



@Injectable({
  providedIn: CoreModule
})
export class ConfigService {

  configData: any = null;

  
  getValueByKey(key: string): any {
    if (key == 'baseUrl') {
      if (this.configData && this.configData[key])
        return this.configData[key];
      else return null;
    }
    else return null;
  }

  constructor(private http: HttpClient, private location: Location) {
    //this.configData = { baseUrl: 'http://localhost:8787/api' }; 
    //http://localhost:61044/api
   
  }

  async getConfig() {
    return this.http.get('./assets/config.json').pipe(
      tap((result) => {
        this.configData = result;
        //alert(JSON.stringify(result));
      })
    ).toPromise();
  }

}
