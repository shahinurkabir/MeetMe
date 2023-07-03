import { Routes } from "@angular/router";
import { AccountSettingsShellComponent } from "./account-settings-shell/account-settings-shell.component";
import { LinkComponent } from "./link/link.component";
import { ProfileComponent } from "./profile/profile.component";

export const accountSettings_Routes: Routes = [
  {
    path: "", component: AccountSettingsShellComponent, children: [
      { path: "", redirectTo: 'profile', pathMatch: 'full' },
      { path: "profile", component: ProfileComponent },
      { path: "link", component: LinkComponent }
    ]
  },
];
export const accountSettings_Components = [
  AccountSettingsShellComponent,
  ProfileComponent,
  LinkComponent
];
