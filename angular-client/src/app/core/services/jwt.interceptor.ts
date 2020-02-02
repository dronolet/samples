import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take, defaultIfEmpty } from 'rxjs/operators';


@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

  constructor() { }

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    //req = req.clone({
    //  withCredentials: true
    //});
    
    return next.handle(req);
  }

}
