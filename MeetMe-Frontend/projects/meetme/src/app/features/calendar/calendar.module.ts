import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppCoreModule } from '../../app-core/app-core.module';
import { calendar_Components, calendar_Routes } from '.';

@NgModule({
    declarations: [
        calendar_Components,
    ],
    providers: [
    ],
    bootstrap: [],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppCoreModule,
        RouterModule.forChild(calendar_Routes)
    ]
})
export class CalendarModule { }
