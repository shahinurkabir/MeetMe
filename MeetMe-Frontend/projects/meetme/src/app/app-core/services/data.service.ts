import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export abstract class DataService {

  constructor(protected http: HttpClient) { }

  doGet(url: string, options?: any): Observable<any> {
    return this.http.get(url, options);
  }

  doPost(url: string, body: any, options?: any): Observable<any> {
    let bodyStrigify = JSON.stringify(body);
    return this.http.post<boolean>(url, bodyStrigify, options);
  }
  doPut(url: string, body: any, options?: any): Observable<any> {
    let bodyStrigify = JSON.stringify(body);
    return this.http.put<boolean>(url, bodyStrigify, options);
  }

  // protected setHeaders(options: any) {
  //   options["headers"] = new HttpHeaders()
  //     .append('Accept', 'application/json')
  //     .append('Content-Type', 'application/json')
  //   //.append('Authorization', `bearer ${this.auth.getToken()}`
  // }
}
