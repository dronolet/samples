import { Injectable, ErrorHandler } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GlobalErrorHandler implements ErrorHandler {

  handleError(error) {
    alert('error');
    // your custom error handling logic    
  }
}
