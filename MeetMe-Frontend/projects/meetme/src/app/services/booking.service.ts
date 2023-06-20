import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IEventTimeAvailability } from "../models/calendar";

@Injectable({
    providedIn: 'root'
})
export class BookingService extends DataService {
    baseURI = `${environment.apiBaseURI}/booking`;

    constructor(http: HttpClient) {
        super(http)
    }
    getList(eventTypeId:string,timezone: string, from: string, to: string): Observable<Array<IEventTimeAvailability>> {
        let url: string = `${this.baseURI}/calendar/event-type/${eventTypeId}?timezone=${timezone}&from=${from}&to=${to}`
        return this.doGet(url)
    }
}