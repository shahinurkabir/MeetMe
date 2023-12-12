import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';
import { DataService } from './data.service';
import { ICreateEventTypeCommand, IUpdateEventCommand, IUpdateEventAvailabilityCommand, IUpdateEventQuestionCommand } from '../interfaces/event-type-commands';
import { IEventAvailabilityDetailItemDto, IEventType, IEventTypeQuestion, IUserProfileDetailResponse } from '../interfaces/event-type-interfaces';
import { IEventTimeAvailability } from '../interfaces/calendar-interface';
import { environment } from 'projects/meetme/src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventTypeService extends DataService {

  eventTypeURI = `${environment.apiBaseURI}/eventtype`;
  eventTypeAvailabilityURI = `${environment.apiBaseURI}/EventTypeAvailability`;
  eventTypeQuestionURI = `${environment.apiBaseURI}/eventtypeQuestion`;

  constructor(http: HttpClient) {
    super(http)
  }

  getList(): Observable<Array<IEventType>> {
    let url = `${this.eventTypeURI}/me`
    return this.http.get<Array<IEventType>>(url);
  }

  getListByUserSlug(base_uri: string): Observable<IUserProfileDetailResponse> {
    let url = `${this.eventTypeURI}/user/${base_uri}`
    return this.http.get<IUserProfileDetailResponse>(url);
  }
  getById(id: string): Observable<IEventType> {
    let url = `${this.eventTypeURI}/detailById/${id}`
    return this.http.get<IEventType>(url);
  }

  getBySlugName(slug: string): Observable<IEventType> {
    let url = `${this.eventTypeURI}/detailBySlug/${slug}`
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
    let url = `${this.eventTypeAvailabilityURI}/${eventTypeId}`
    return this.http.get<Array<IEventAvailabilityDetailItemDto>>(url);
  }

  updateAvailability(availabilityDetail: IUpdateEventAvailabilityCommand): Observable<boolean> {
    let url = `${this.eventTypeAvailabilityURI}/${availabilityDetail.id}`;
    return this.doPost(url, availabilityDetail);

  }

  getEventQuetions(eventTypeId: string): Observable<Array<IEventTypeQuestion>> {
    let url = `${this.eventTypeQuestionURI}/${eventTypeId}`
    return this.http.get<Array<IEventTypeQuestion>>(url);
  }

  updateEventQuestions(updateEventQuestionsCommand: IUpdateEventQuestionCommand): Observable<boolean> {
    let url = `${this.eventTypeQuestionURI}/${updateEventQuestionsCommand.eventTypeId}`
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

  getEventAvailabilityCalendar(event_slug: string, timezone: string, from: string, to: string): Observable<Array<IEventTimeAvailability>> {
    let url: string = `${this.eventTypeURI}/calendar-availability?eventslug=${event_slug}&timezone=${timezone}&from=${from}&to=${to}`
    return this.doGet(url)
  }
  toggleStatusMultiple(ids: string[]):Observable<boolean> {
    throw new Error('Method not implemented.');
  }
  deleteMultiple(ids: string[]):Observable<boolean> {
    throw new Error('Method not implemented.');
  }
}
