import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ICreateEventTypeCommand, EventType, IUpdateEventAvailabilityCommand, IEventTypeQuestion, IUpdateEventQuestionCommand, TimeZoneData, IUpdateEventCommand } from '../models/eventtype';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root'
})
export class TimeZoneService extends DataService {
  timeZoneURI = `${environment.apiBaseURI}/timezone`;

  constructor( http: HttpClient) {
      super(http);
  }

  
  getList(): Observable<Array<any>> {
    //let headers = {};
   // this.setHeaders(headers);
    let url = `${this.timeZoneURI}`
    return this.http.get<Array<any>>(url);
  }

  getByName(name: string): Observable<TimeZoneData> {
    //let headers = {};
    //this.setHeaders(headers);
    let url = `${this.timeZoneURI}/${name}`
    return this.http.get<TimeZoneData>(url);
  }

}
