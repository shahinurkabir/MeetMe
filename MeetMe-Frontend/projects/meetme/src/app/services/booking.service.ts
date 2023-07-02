import { Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IEventTimeAvailability } from "../interfaces/calendar";

@Injectable({
    providedIn: 'root'
})
export class BookingService extends DataService {
    baseURI = `${environment.apiBaseURI}/booking`;

    constructor(http: HttpClient) {
        super(http)
    }
    
}