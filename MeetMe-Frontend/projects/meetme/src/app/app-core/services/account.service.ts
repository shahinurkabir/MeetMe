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
    updateUri(updateLinkCommand: IUpdateUserLinkCommand): Observable<IUpdateAccountSettingsResponse> {
        let url = `${this.baseURI}/update-uri`;
        return this.doPost(url, updateLinkCommand);
    }

    isUriAvailable(link: string): Observable<boolean> {
        let url = `${this.baseURI}/uri-available/${link}`;
        return this.doGet(url);
    }

    getUserById(id:string): Observable<IAccountProfileInfo> {
        let url = `${this.baseURI}/userById/${id}}`;
        return this.doGet(url);
    }
    getProfileByUserName(name:string): Observable<IAccountProfileInfo> {
        let url = `${this.baseURI}/profile/${name}`;
        return this.doGet(url);
    }

}