import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';
import { CoreModule } from '../core.module';
import { ConfigService } from '../services/config.service';



@Injectable({
  providedIn: CoreModule
})
export class AuthService {

  

  constructor(private http: HttpClient, private config: ConfigService ) { } 
  
  sign(data):any {
    return this.http.post(this.config.getValueByKey('baseUrl') + '/auth/sign/?r=' + Math.random(), data)
  }

  remember(data): any {
    return this.http.post(this.config.getValueByKey('baseUrl') + '/auth/remember/?r=' + Math.random(), data)
  }

  signOut(): any {
    return this.http.post(this.config.getValueByKey('baseUrl') + '/auth/signout/?r=' + Math.random(), null)
  }

  getCurrentUser(): any {
    return this.http.get(this.config.getValueByKey('baseUrl') + '/auth/info/?r=' + Math.random())
  }


}
