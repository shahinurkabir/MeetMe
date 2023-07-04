import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';
import { DataService } from './data.service';
import { ICreateEventTypeCommand, IUpdateEventCommand, IUpdateEventAvailabilityCommand, IUpdateEventQuestionCommand } from '../interfaces/event-type-commands';
import { IEventAvailabilityDetailItemDto, IEventType, IEventTypeQuestion, IUserProfileDetailResponse } from '../interfaces/event-type-interfaces';
import { IEventTimeAvailability } from '../interfaces/calendar';
import { environment } from 'projects/meetme/src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventTypeService extends DataService {
  eventTypeURI = `${environment.apiBaseURI}/eventtypes`;

  constructor(http: HttpClient) {
    super(http)
  }

  getList(): Observable<Array<IEventType>> {
    let url = `${this.eventTypeURI}`
    return this.http.get<Array<IEventType>>(url);
  }
  getListByBaseURI(base_uri:string): Observable<IUserProfileDetailResponse> {
    let url = `${this.eventTypeURI}/list/${base_uri}`
    return this.http.get<IUserProfileDetailResponse>(url);
  }
  getById(id: string): Observable<IEventType> {
    let url = `${this.eventTypeURI}/${id}`
    return this.http.get<IEventType>(url);
  }
  addNew(command: ICreateEventTypeCommand): Observable<string> {
    let url = `${this.eventTypeURI}`

    return this.doPost(url, command);

  }

  update(updateEventType: IUpdateEventCommand): Observable<boolean> {
    let url = `${this.eventTypeURI}/${updateEventType.id}`;

    return this.doPut(url, updateEventType);

  }

  getEventAvailability(eventTypeId: string): Observable<Array<IEventAvailabilityDetailItemDto>> {
    let url = `${this.eventTypeURI}/${eventTypeId}/availability`
    return this.http.get<Array<IEventAvailabilityDetailItemDto>>(url);
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
  toggleStatus(eventTypeId: string): Observable<boolean> {
    let url = `${this.eventTypeURI}/${eventTypeId}/toggle-status`
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

  getCalendarAvailability(eventTypeId: string, timezone: string, from: string, to: string): Observable<Array<IEventTimeAvailability>> {
    let url: string = `${this.eventTypeURI}/${eventTypeId}/calendar-availability?timezone=${timezone}&from=${from}&to=${to}`
    return this.doGet(url)
  }
}