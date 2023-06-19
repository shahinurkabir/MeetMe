import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WorkinghoursComponent } from './features/workinghours/workinghours.component';
import { RouterModule } from '@angular/router';
import { HttpRequestInterceptor } from './interceptors/http-interceptor';
import { LoginComponent } from './features/users/login/login.component';
import { AvailabilityComponent } from './features/availability/availability.component';
import { TimeAvailabilityComponent } from './controls/time-availability/time-availability.component';
import { CalendarComponent } from './controls/calender/calendar.component';
import { EventTypeListComponent } from './features/event-types/event-type-list/event-type-list.component';
import { EventAvailabilityComponent } from './features/event-types/event-type/event-availability/event-availability.component';
import { EventInfoNewComponent } from './features/event-types/event-type/event-info/event-info-new/event-info-new.component';
import { EventInfoUpdateComponent } from './features/event-types/event-type/event-info/event-info-update/event-info-update.component';
import { EventInfoComponent } from './features/event-types/event-type/event-info/event-info.component';
import { EventQuestionComponent } from './features/event-types/event-type/event-question/event-question.component';
import { EventTypeComponent } from './features/event-types/event-type/eventtype.component';
import { AvailabilityListComponent } from './features/availability/availability-list/availability-list.component';
import { ModalComponent } from './controls/modal/modal.component';
import { MyOffClickDirective } from './directives/myOffClickDirective';
import { CallbackPipe } from './pipes/callback-pipe';
import { DistributionComponent } from './distribution/distribution/distribution.component';
import { NameComponent } from './distribution/cards/name/name.component';
import { AddressComponent } from './distribution/cards/address/address.component';
import { EventTypeCalendarComponent } from './features/eventtype-calendar/eventtype-calendar.component';
import { TimezoneControlComponent } from './controls/timezone-control/timezone-control.component';
import { TestComponentComponent } from './test-component/test-component.component';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        WorkinghoursComponent,
        LoginComponent,
        AvailabilityComponent,
        TimeAvailabilityComponent,
        CalendarComponent,
        EventTypeListComponent,
        EventTypeComponent,
        EventInfoComponent,
        EventAvailabilityComponent,
        EventQuestionComponent,
        EventInfoNewComponent,
        EventInfoUpdateComponent,
        AvailabilityListComponent,
        ModalComponent,
        MyOffClickDirective,
        CallbackPipe,
        DistributionComponent,
        NameComponent,
        AddressComponent,
        CalendarComponent,
        EventTypeCalendarComponent,
        TimezoneControlComponent,
        TestComponentComponent
        
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: HttpRequestInterceptor, multi: true },
    ],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppRoutingModule,
        HttpClientModule,
    ]
})
export class AppModule { }
