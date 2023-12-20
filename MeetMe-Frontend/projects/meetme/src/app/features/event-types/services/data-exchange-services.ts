import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({ providedIn: 'root' })
export class DataExchangeService {
    private _eventTitleSubject = new Subject<string>();
    setEventTypeTitle(title: string) {
        this._eventTitleSubject.next(title);
    }
    getEventTypeTitle() {
        return this._eventTitleSubject.asObservable();
    }
}