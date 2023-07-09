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
import { AuthService } from '../services/auth-service';
import { AlertService } from '../controls/alert/alert.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private auth: AuthService,
    private alertService: AlertService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    request = request.clone({
      setHeaders: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `bearer ${this.auth.accessToken}`

      }
    });

    return next.handle(request).pipe(catchError(x => this.handleAuthError(x)));
  }
  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    if (err.status === 401 || err.status === 403) {
      this.router.navigateByUrl(`/login`);
      return of(err.message);
    }
    else if (err.status === 400) {
      this.alertService.info("Bad request");  
    }
    else if (err.status === 500) {
      this.alertService.error(err.error.title);
    }
    else {
      this.alertService.error(err.statusText);
    }
    return throwError(() => err);
  }
}
