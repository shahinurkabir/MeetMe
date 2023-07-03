import { Injectable } from "@angular/core";
import { IUpdateProfileCommand, IUpdateUserLinkCommand } from "../interfaces/account-commands";
import { DataService } from "./data.service";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { IAccountProfileInfo, IUpdateAccountSettingsResponse } from "../interfaces/account-interfaces";
import { environment } from "projects/meetme/src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class AccountService extends DataService {
    
    baseURI = `${environment.apiBaseURI}/account`;

    constructor(http: HttpClient) {
        super(http)
    }
    getProfile(): Observable<IAccountProfileInfo> {
        let url = `${this.baseURI}/profile`;
        return this.doGet(url);
    }
    updateProfile(updateProfileCommand: IUpdateProfileCommand): Observable<IUpdateAccountSettingsResponse> {
        let url = `${this.baseURI}/profile`;
        return this.doPost(url, updateProfileCommand);
    }
    updateLink(updateLinkCommand: IUpdateUserLinkCommand): Observable<IUpdateAccountSettingsResponse> {
        let url = `${this.baseURI}/link`;
        return this.doPost(url, updateLinkCommand);
    }

    isLinkAvailable(link: string): Observable<boolean> {
        let url = `${this.baseURI}/link-available/${link}`;
        return this.doGet(url);
    }

}