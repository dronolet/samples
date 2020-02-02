import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from 'src/app/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/internal/operators';


@Injectable({ providedIn: 'root' })
export class MainResolve implements Resolve<any> {

  constructor(private authService: AuthService) { }

  resolve(route: ActivatedRouteSnapshot) {
   
    return this.authService.getCurrentUser().pipe(
      catchError(error => {        
        console.error(`Can't resolve user authentithication`);
        return of(null);
      })
    );
    
  }
}

  
