import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './features/users/login/login.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { eventType_Components } from './features/event-types';
import { TimeAvailabilityComponent, CalendarComponent, ModalComponent, MyOffClickDirective, TimezoneControlComponent, HttpRequestInterceptor } from './app-core';
import { CallbackPipe } from './app-core/pipes/callback-pipe';
import { accountSettings_Components } from './features/acccount-settings';
import { availability_Components } from './features/availability';
import { calendar_Components } from './features/calendar';
import { CalendarLayoutComponent } from './layouts/calendar-layout/calendar-layout.component';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        TimeAvailabilityComponent,
        CalendarComponent,
        ModalComponent,
        MyOffClickDirective,
        CallbackPipe,
        TimezoneControlComponent,
        AuthLayoutComponent,
        AdminLayoutComponent,
        CalendarLayoutComponent,
        accountSettings_Components,
        availability_Components,
        eventType_Components,
        calendar_Components
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
