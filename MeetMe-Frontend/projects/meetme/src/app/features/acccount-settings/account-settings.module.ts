import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountSettingsShellComponent } from './account-settings-shell/account-settings-shell.component';
import { RouterModule, Routes } from '@angular/router';
import { LinkComponent } from './link/link.component';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const routes: Routes = [
  {
    path: "", component: AccountSettingsShellComponent, children: [
      { path: "", redirectTo: 'profile', pathMatch: 'full' },
      { path: "profile", component: ProfileComponent },
      { path: "link", component: LinkComponent }
    ]
  },
];

@NgModule({
  declarations: [
    AccountSettingsShellComponent,
    ProfileComponent,
    LinkComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes)
  ]
})
export class AccountSettingsModule { }
