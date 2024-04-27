import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AlertComponent, CalendarComponent,  MultiCalendarComponent, MyModalComponent, TimeAvailabilityComponent, TimezoneControlComponent, ToggleSwitchComponent,TooltipComponent, TooltipDirective } from './controls';
import { MyOffClickDirective, LoadingIndicatorDirective } from './directives';
import { CallbackPipe, FilterPipe } from './pipes';


@NgModule({
    declarations: [
        TimeAvailabilityComponent,
        CalendarComponent,
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
        TooltipComponent,
        TooltipDirective
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
        TooltipComponent,
        TooltipDirective
    ]
})
export class AppCoreModule { }
