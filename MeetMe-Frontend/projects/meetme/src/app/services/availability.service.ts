import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { CloneAvailabilityCommand } from "../commands/cloneAvailabilityCommand";
import { CreateAvailabilityCommand } from "../commands/createAvailabilityCommand";
import { DeleteAvailabilityCommand, SetDefaultAvailabilityCommand } from "../commands/deleteAvailabilityCommand";
import { EditAvailabilityNameCommand } from "../commands/editAvailabilityNameCommand";
import { IAvailability } from "../models/IAvailability";
import { DataService } from "./data.service";

@Injectable({
    providedIn: 'root'
})
export class AvailabilityService extends DataService {
    baseURI = `${environment.apiBaseURI}/availability`;

    constructor(http: HttpClient) {
        super(http)
    }
    getList(): Observable<Array<IAvailability>> {
        let url: string = `${this.baseURI}`
        return this.doGet(url)
    }

    addNew(command: CreateAvailabilityCommand): Observable<string> {
        let url: string = `${this.baseURI}`
        return this.doPost(url, command)
    }

    editName(command: EditAvailabilityNameCommand): Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/editname`;
        return this.doPost(url, command);
    }
    clone(command: CloneAvailabilityCommand): Observable<string> {
        let url: string = `${this.baseURI}/${command.id}/clone`;
        return this.doPost(url, command);
    }

    edit(command: EditAvailabilityNameCommand): Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}`;
        return this.doPut(url, command);
    }
    delete(command:DeleteAvailabilityCommand):Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/delete`;
        return this.doPost(url, command);
    }
    setDefault(command:SetDefaultAvailabilityCommand):Observable<boolean> {
        let url: string = `${this.baseURI}/${command.id}/default`;
        return this.doPost(url, command);
    }
}