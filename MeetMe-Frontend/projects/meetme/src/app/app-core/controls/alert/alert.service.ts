import { Injectable, inject } from "@angular/core";
import { Subject } from "rxjs";
import { Alert, AlertType } from "./alert";

@Injectable({
    providedIn: 'root'
})
export class AlertService {
    private subject:Subject<Alert>=new Subject<Alert>;
    readonly alert$=this.subject.asObservable();

    constructor() { 
        console.log("auth service");
    }

    info(message:string){
        this.onAlert(new Alert(message,AlertType.Info));
    }
    success(message:string){
        this.onAlert(new Alert(message,AlertType.Success));
    }
    error(message:string){
        this.onAlert(new Alert(message,AlertType.Error));
    }
    warning(message:string){    
        this.onAlert(new Alert(message,AlertType.Warning));
    }
    private onAlert(alert:Alert){
        this.subject.next(alert);
    }
}