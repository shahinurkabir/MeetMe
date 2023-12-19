import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { eventType_Components, eventType_Routes } from '.';
import { AppCoreModule } from '../../app-core/app-core.module';

@NgModule({
    declarations: [
        eventType_Components
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
        RouterModule.forChild(eventType_Routes)
    ]
})
export class EventTypeModule { }
