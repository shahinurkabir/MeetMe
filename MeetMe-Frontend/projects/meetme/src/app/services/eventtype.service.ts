import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { CreateEventTypeCommand, EventType, EventTypeAvailability, IEventTypeQuestion, IUpdateEventQuestionCommand, UpdateEventCommand } from '../models/eventtype';
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
  addNew(command: CreateEventTypeCommand): Observable<string> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}`

    return this.doPost(url, command);

  }

  update(updateEventType: UpdateEventCommand): Observable<boolean> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${updateEventType.id}`;
    //let body = JSON.stringify(updateEventType);

    return this.doPut(url, updateEventType);

  }

  getEventAvailability(eventTypeId: string): Observable<EventTypeAvailability> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${eventTypeId}/availability`
    return this.http.get<EventTypeAvailability>(url);
  }

  updateAvailability(availabilityDetail: EventTypeAvailability): Observable<boolean> {
    // let headers = {};
    // this.setHeaders(headers);
    let url = `${this.eventTypeURI}/${availabilityDetail.id}/availability`;
    //let body = JSON.stringify(availabilityDetail);

    return this.http.post<boolean>(url, availabilityDetail);

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
