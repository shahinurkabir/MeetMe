import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppCoreModule } from '../../app-core/app-core.module';
import { scheduled_Appointment_Components, scheduled_Appointment_Routes } from './';

@NgModule({
    declarations: [
        scheduled_Appointment_Components
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
        RouterModule.forChild(scheduled_Appointment_Routes)
    ]
})
export class ScheduleEventsModule { }
