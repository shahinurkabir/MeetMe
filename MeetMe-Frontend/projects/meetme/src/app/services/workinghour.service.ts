// import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Observable } from 'rxjs';
// import { ScheduleRule } from '../interfaces/schedule-rule';

// @Injectable({
//   providedIn: 'root'
// })
// export class WorkinghourService {

//   url="http://localhost:5073/api/ScheduleRule";
//   constructor(private http: HttpClient) { }

//   getList():Observable<Array<ScheduleRule>>{
//     let headers={};
//     this.setHeaders(headers);
//     let url=`${this.url}`
//     return this.http.get<Array<ScheduleRule>>(url,headers);
//   }

//   private setHeaders (options:any) {
//     options["headers"]=new HttpHeaders()
//         .append('Accept', 'application/json')
//         .append('Content-Type', 'application/json')
//         //.append('Authorization', `bearer ${this.auth.getToken()}`
//   }
// }
