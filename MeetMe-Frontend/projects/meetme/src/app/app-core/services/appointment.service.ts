import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "projects/meetme/src/environments/environment";
import { ICancelAppointmentCommand } from "../interfaces";

@Injectable({
    providedIn: 'root'
})
export class AppointmentService extends DataService {
    calenarURI = `${environment.apiBaseURI}/appointment`;

    constructor(http: HttpClient) {
        super(http)
    }
    
    addAppointment(command: any): Observable<string> {
        let url: string = `${this.calenarURI}/new`
        return this.doPost(url, command)
    }
    cancelAppointment(command: ICancelAppointmentCommand): Observable<string> {
        let url: string = `${this.calenarURI}/cancel`
        return this.doPost(url, command)
    }
}