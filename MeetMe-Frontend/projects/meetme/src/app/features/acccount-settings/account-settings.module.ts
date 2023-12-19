import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppCoreModule } from '../../app-core/app-core.module';
import { accountSettings_Components, accountSettings_Routes } from '.';

@NgModule({
    declarations: [
        accountSettings_Components
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
        RouterModule.forChild(accountSettings_Routes)
    ]
})
export class AccountSettingsModule { }
