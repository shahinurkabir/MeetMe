import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { availability_Components, availability_Routes } from '.';
import { AppCoreModule } from '../../app-core/app-core.module';

@NgModule({
    declarations: [
        availability_Components
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
        RouterModule.forChild(availability_Routes)
    ]
})
export class AvailabilityModule { }
