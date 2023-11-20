import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "projects/meetme/src/environments/environment";
import { IAppointmentDetailsDto, IAppointmentSearchParametersDto, IAppointmentsPaginationResult, ICancelAppointmentCommand, ICreateAppointmentCommand } from "../interfaces";
import { NumberValueAccessor } from "@angular/forms";

@Injectable({
    providedIn: 'root'
})
export class AppointmentService extends DataService {
    calendarURI = `${environment.apiBaseURI}/appointment`;

    constructor(http: HttpClient) {
        super(http)
    }
    
    getScheduleEvents(queryParams:string): Observable<IAppointmentsPaginationResult> {
        let url: string = `${this.calendarURI}/schedule-event?${queryParams}`
        return this.doGet(url,)
    }
    getAppointmentById(id: string): Observable<IAppointmentDetailsDto> {
        let url: string = `${this.calendarURI}/${id}/details`
        return this.doGet(url)
    }
    addAppointment(command: ICreateAppointmentCommand): Observable<string> {
        let url: string = `${this.calendarURI}/new`
        return this.doPost(url, command)
    }
    cancelAppointment(command: ICancelAppointmentCommand): Observable<string> {
        let url: string = `${this.calendarURI}/${command.id}/cancel`
        return this.doPost(url, command)
    }
}