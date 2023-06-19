import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IEventTimeAvailability } from "../models/calendar";

@Injectable({
    providedIn: 'root'
})
export class CalendarService extends DataService {
    baseURI = `${environment.apiBaseURI}/calendar`;

    constructor(http: HttpClient) {
        super(http)
    }
    getList(timezone: string, from: string, to: string): Observable<Array<IEventTimeAvailability>> {
        let url: string = `${this.baseURI}/getlist?timezone=${timezone}&from=${from}&to=${to}`
        return this.doGet(url)
    }
}