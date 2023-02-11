import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, of, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        //'Authorization': `bearer ${this.auth.getToken()}`

      }
    });

    return next.handle(request).pipe(catchError(x => this.handleAuthError(x)));
  }
  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    if (err.status === 401 || err.status === 403) {
      this.router.navigateByUrl(`/auth/login`);
      return of(err.message);
    }
    else if (err.status === 400) {
     // this.toasterMessageService.ShowInfo(err.error.title);
    }
    else if (err.status === 500) {
     // this.toasterMessageService.ShowError(err.error.title);
    }
    else {
     // this.toasterMessageService.ShowError(err.statusText);
    }
    return throwError(() => err);
  }
}
