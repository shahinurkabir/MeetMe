import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { ICreateEventTypeCommand, EventType, IUpdateEventAvailabilityCommand, IEventTypeQuestion, IUpdateEventQuestionCommand, IUpdateEventCommand } from '../models/eventtype';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root'
})
export class EventTypeService extends DataService {
  eventTypeURI = `${environment.apiBaseURI}/eventtypes`;

  constructor(http: HttpClient) {
    super(http)
  }

  getList(): Observable<Array<EventType>> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}`
    return this.http.get<Array<EventType>>(url);
  }

  getById(id: string): Observable<EventType> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${id}`
    return this.http.get<EventType>(url);
  }
  addNew(command: ICreateEventTypeCommand): Observable<string> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}`

    return this.doPost(url, command);

  }

  update(updateEventType: IUpdateEventCommand): Observable<boolean> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${updateEventType.id}`;
    //let body = JSON.stringify(updateEventType);

    return this.doPut(url, updateEventType);

  }

  getEventAvailability(eventTypeId: string): Observable<IUpdateEventAvailabilityCommand> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${eventTypeId}/availability`
    return this.http.get<IUpdateEventAvailabilityCommand>(url);
  }

  updateAvailability(availabilityDetail: IUpdateEventAvailabilityCommand): Observable<boolean> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${availabilityDetail.id}/availability`;
    //let body = JSON.stringify(availabilityDetail);

    return this.doPost(url, availabilityDetail);

  }

  getEventQuetions(eventTypeId: string): Observable<Array<IEventTypeQuestion>> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${eventTypeId}/questions`
    return this.http.get<Array<IEventTypeQuestion>>(url);
  }

  updateEventQuestions(updateEventQuestionsCommand: IUpdateEventQuestionCommand): Observable<boolean> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${updateEventQuestionsCommand.eventTypeId}/questions`
    return this.doPost(url, updateEventQuestionsCommand);
  }

  // private doGet(url: string, options: any): Observable<any> {
  //   return this.http.get(url, options);
  // }

  // private doPost(url: string, body: any, options?: any): Observable<any> {
  //   let bodyStrigify = JSON.stringify(body);
  //   return this.http.post<boolean>(url, bodyStrigify, options);
  // }
  // private doPut(url: string, body: any, options?: any): Observable<any> {
  //   let bodyStrigify = JSON.stringify(body);
  //   return this.http.put<boolean>(url, bodyStrigify, options);
  // }
  // private setHeaders(options: any) {
  //   options["headers"] = new HttpHeaders()
  //     .append('Accept', 'application/json')
  //     .append('Content-Type', 'application/json')
  //   //.append('Authorization', `bearer ${this.auth.getToken()}`
  // }
}
