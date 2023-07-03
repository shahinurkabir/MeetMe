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
import { BookingLayoutComponent } from './layouts/booking-layout/booking-layout.component';
import { eventType_Components } from './features/event-types';
import { TimeAvailabilityComponent, CalendarComponent, ModalComponent, MyOffClickDirective, TimezoneControlComponent, HttpRequestInterceptor } from './app-core';
import { CallbackPipe } from './app-core/pipes/callback-pipe';
import { booking_Components } from './features/booking';
import { accountSettings_Components } from './features/acccount-settings';
import { availability_Components } from './features/availability';

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
        BookingLayoutComponent,
        accountSettings_Components,
        availability_Components,
        eventType_Components,
        booking_Components
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
