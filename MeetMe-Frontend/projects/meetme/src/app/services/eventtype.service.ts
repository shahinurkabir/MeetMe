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
    let url = `${this.eventTypeURI}`
    return this.http.get<Array<EventType>>(url);
  }

  getById(id: string): Observable<EventType> {
    let url = `${this.eventTypeURI}/${id}`
    return this.http.get<EventType>(url);
  }
  addNew(command: ICreateEventTypeCommand): Observable<string> {
    let url = `${this.eventTypeURI}`

    return this.doPost(url, command);

  }

  update(updateEventType: IUpdateEventCommand): Observable<boolean> {
    let url = `${this.eventTypeURI}/${updateEventType.id}`;

    return this.doPut(url, updateEventType);

  }

  getEventAvailability(eventTypeId: string): Observable<IUpdateEventAvailabilityCommand> {
    let url = `${this.eventTypeURI}/${eventTypeId}/availability`
    return this.http.get<IUpdateEventAvailabilityCommand>(url);
  }

  updateAvailability(availabilityDetail: IUpdateEventAvailabilityCommand): Observable<boolean> {
    let url = `${this.eventTypeURI}/${availabilityDetail.id}/availability`;
    return this.doPost(url, availabilityDetail);

  }

  getEventQuetions(eventTypeId: string): Observable<Array<IEventTypeQuestion>> {
    let url = `${this.eventTypeURI}/${eventTypeId}/questions`
    return this.http.get<Array<IEventTypeQuestion>>(url);
  }

  updateEventQuestions(updateEventQuestionsCommand: IUpdateEventQuestionCommand): Observable<boolean> {
    let url = `${this.eventTypeURI}/${updateEventQuestionsCommand.eventTypeId}/questions`
    return this.doPost(url, updateEventQuestionsCommand);
  }
  toggleActive(eventTypeId: string): Observable<boolean> {
    let url = `${this.eventTypeURI}/${eventTypeId}/toggleactive`
    return this.doPut(url, null);
  }
  clone(eventTypeId: string): Observable<string> {
    let url = `${this.eventTypeURI}/${eventTypeId}/clone`
    return this.doPut(url, null);
  }
  delete(eventTypeId: string): Observable<boolean> {
    let url = `${this.eventTypeURI}/${eventTypeId}/delete`
    return this.doPut(url, null);
  }
}
