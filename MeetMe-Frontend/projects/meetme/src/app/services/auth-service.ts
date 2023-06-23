import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';

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
        localStorage.removeItem(this.TOKEN_KEY);
        this.router.navigate([`login`]);
    }

    onLogin(loginInfo: { userId: string, password: string }): Observable<any> {

        let url = environment.apiBaseURI + "/account/token";
        let body = JSON.stringify(loginInfo);

        let response = this.http.post(url, body).pipe(map((response: any) => this.loginComplete(response)))

        return response;
    }

    private loginComplete(response: any): any {
        
        if (response==null) return response;

        this.setTokenData(response);
        
        return response
    }

    private setTokenData(response: any) {
        let tokenData = {
            token: response.token,
            expiredAt:(Math.floor((new Date).getTime() / 1000)) + response.expiredAt 
        };
        let data = JSON.stringify(tokenData);
        localStorage.setItem(this.TOKEN_KEY, data);
    }

    private getTokenData() {
        let tokenInfo = localStorage.getItem(this.TOKEN_KEY);
        if (tokenInfo) {
            return JSON.parse(tokenInfo);
        }
        return null;
    }
    private isTokenExpired(): boolean {
        var data = this.getTokenData();
        if (data && data.token && data.expiredAt) {
            let expiryTime = parseInt(data.expiredAt);
            let isExpired = (Math.floor((new Date).getTime() / 1000) > expiryTime)
            if (!isExpired)
                return false;
        }
        return true;
    }
    get  accessToken():string {
        var data = this.getTokenData();
        if (data && data.token && data.expiredAt) {
            let expiryTime = parseInt(data.expiredAt);
            let isExpired = (Math.floor((new Date).getTime() / 1000) > expiryTime)
            if (!isExpired)
                return data.token;
        }
        return "";
    }
}