import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AlertComponent, CalendarComponent, ModalComponent, MultiCalendarComponent, MyModalComponent, TimeAvailabilityComponent, TimezoneControlComponent, ToggleSwitchComponent } from './controls';
import { MyOffClickDirective, LoadingIndicatorDirective } from './directives';
import { CallbackPipe, FilterPipe } from './pipes';
import { BrowserModule } from '@angular/platform-browser';


@NgModule({
    declarations: [
        TimeAvailabilityComponent,
        CalendarComponent,
        ModalComponent,
        MyModalComponent,
        MyOffClickDirective,
        CallbackPipe,
        FilterPipe,
        TimezoneControlComponent,
        AlertComponent,
        LoadingIndicatorDirective,
        MultiCalendarComponent,
        MyModalComponent,
        ToggleSwitchComponent,
    ],
    providers: [
    ],
    bootstrap: [],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpClientModule,
    ],
    exports: [
        TimeAvailabilityComponent,
        CalendarComponent,
        ModalComponent,
        MyModalComponent,
        MyOffClickDirective,
        CallbackPipe,
        FilterPipe,
        TimezoneControlComponent,
        AlertComponent,
        LoadingIndicatorDirective,
        MultiCalendarComponent,
        MyModalComponent,
        ToggleSwitchComponent,
    ]
})
export class AppCoreModule { }
