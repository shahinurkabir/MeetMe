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

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        WorkinghoursComponent,
        LoginComponent,
        //CalenderComponent
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
