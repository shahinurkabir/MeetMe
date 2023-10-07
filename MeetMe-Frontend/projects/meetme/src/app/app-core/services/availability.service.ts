import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DataService } from "./data.service";
import { ICreateAvailabilityCommand, IEditAvailabilityNameCommand, ICloneAvailabilityCommand, IDeleteAvailabilityCommand, ISetDefaultAvailabilityCommand } from "../interfaces/availability-commands";
import { IAvailability } from "../interfaces/availability-interfaces";
import { environment } from "projects/meetme/src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class AvailabilityService extends DataService {
    baseURI = `${environment.apiBaseURI}/availability`;

    constructor(http: HttpClient) {
        super(http)
    }
    getList(): Observable<Array<IAvailability>> {
        let url: string = `${this.baseURI}/me`
        return this.doGet(url)
    }

    addNew(command: ICreateAvailabilityCommand): Observable<string> {
        let url: string = `${this.baseURI}`
        return this.doPost(url, command)
    }

    editName(command: IEditAvailabilityNameCommand): Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/editname`;
        return this.doPost(url, command);
    }
    clone(command: ICloneAvailabilityCommand): Observable<string> {
        let url: string = `${this.baseURI}/${command.id}/clone`;
        return this.doPost(url, command);
    }

    edit(command: IEditAvailabilityNameCommand): Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}`;
        return this.doPut(url, command);
    }
    delete(command:IDeleteAvailabilityCommand):Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/delete`;
        return this.doPost(url, command);
    }
    setDefault(command:ISetDefaultAvailabilityCommand):Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/default`;
        return this.doPost(url, command);
    }
}