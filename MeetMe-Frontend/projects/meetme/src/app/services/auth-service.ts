import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { parseJwt } from '../utilities/functions';
import { ClaimTypes } from '../utilities/keys';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    TOKEN_KEY = "token_data"
    constructor(private http: HttpClient, private router: Router) {

    }

    isLogin() {
        return !this.isTokenExpired();
    }

    logout() {
        localStorage.removeItem(ClaimTypes.ACCESS_TOKEN);
        localStorage.removeItem(ClaimTypes.TOKEN_EXPIRES_AT);
        localStorage.removeItem(ClaimTypes.USER_ID);
        localStorage.removeItem(ClaimTypes.BASE_URI);
        localStorage.removeItem(ClaimTypes.USER_EMAIL);
        localStorage.removeItem(ClaimTypes.USER_TIMEZONE);

        this.router.navigateByUrl(`auth/login`);
    }

    onLogin(loginInfo: { userId: string, password: string }): Observable<any> {

        let url = environment.apiBaseURI + "/account/token";
        let body = JSON.stringify(loginInfo);

        let response = this.http.post(url, body).pipe(map((response: any) => this.loginComplete(response)))

        return response;
    }
    
    resetToken(token:any) {
        this.setTokenData(token);
    }
    private loginComplete(response: any): any {

        if (response == null) return response;

        this.setTokenData(response);

        return response
    }
  
    private setTokenData(response: any) {

        let expiredAt = (Math.floor((new Date).getTime() / 1000)) + response.expiredAt;

        try {
            let jsonString = parseJwt(response.token);
            localStorage.setItem(ClaimTypes.ACCESS_TOKEN, response.token);
            localStorage.setItem(ClaimTypes.TOKEN_EXPIRES_AT, expiredAt);
            localStorage.setItem(ClaimTypes.USER_ID, jsonString["user_id"]);
            localStorage.setItem(ClaimTypes.BASE_URI, jsonString["base_uri"]);
            localStorage.setItem(ClaimTypes.USER_NAME, jsonString["user_name"]);
            localStorage.setItem(ClaimTypes.USER_TIMEZONE, jsonString["user_timezone"]);

        }
        catch (e) {
            console.log(e);
        }
    }

    private isTokenExpired(): boolean {
        let expiredAt = localStorage.getItem(ClaimTypes.TOKEN_EXPIRES_AT);
        if (expiredAt) {
            let expiryTime = parseInt(expiredAt);
            let isExpired = (Math.floor((new Date).getTime() / 1000) > expiryTime)
            if (!isExpired)
                return false;
        }
        return true;
    }
    get accessToken(): string {
        let token = localStorage.getItem(ClaimTypes.ACCESS_TOKEN);
        let expiredAt = localStorage.getItem(ClaimTypes.TOKEN_EXPIRES_AT);
        if (token && expiredAt) {
            let expiryTime = parseInt(expiredAt);
            let isExpired = (Math.floor((new Date).getTime() / 1000) > expiryTime)
            if (!isExpired)
                return token;
        }
        return "";
    }
    get userId(): string {  
        return localStorage.getItem(ClaimTypes.USER_ID) ?? "";
    }
    get userName(): string {
        return localStorage.getItem(ClaimTypes.USER_NAME) ?? "";
    }
    get baseUri(): string {
        return localStorage.getItem(ClaimTypes.BASE_URI) ?? "";
    }
   
    get userTimeZone(): string {
        return localStorage.getItem(ClaimTypes.USER_TIMEZONE) ?? "";
    }
    
}