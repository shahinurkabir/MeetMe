import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IEventTimeAvailability } from "../interfaces/calendar";
import { environment } from "projects/meetme/src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class BookingService extends DataService {
    baseURI = `${environment.apiBaseURI}/booking`;

    constructor(http: HttpClient) {
        super(http)
    }
    
}