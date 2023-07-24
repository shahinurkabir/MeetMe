import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IEventTimeAvailability } from "../interfaces/calendar-interface";
import { environment } from "projects/meetme/src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class CalendarService extends DataService {
    calenarURI = `${environment.apiBaseURI}/calendar`;

    constructor(http: HttpClient) {
        super(http)
    }
    getCalendarAvailability(eventTypeId: string, timezone: string, from: string, to: string): Observable<Array<IEventTimeAvailability>> {
        let url: string = `${this.calenarURI}/availability/${eventTypeId}?timezone=${timezone}&from=${from}&to=${to}`
        return this.doGet(url)
    }
    AddNewAppointment(command: any): Observable<string> {
        let url: string = `${this.calenarURI}/appointment/new`
        return this.doPost(url, command)
    }
}