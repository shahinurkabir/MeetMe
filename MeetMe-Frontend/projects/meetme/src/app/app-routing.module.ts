import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/users/login/login.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { eventType_Routes } from './features/event-types';
import { AuthGuard } from './app-core';
import { accountSettings_Routes } from './features/acccount-settings';
import { availability_Route } from './features/availability';
import { calendar_Routes } from './features/calendar';
import { CalendarLayoutComponent } from './layouts/calendar-layout/calendar-layout.component';
import { scheduled_Appointment_Routes } from './features/scheduled-appointments';

const admin_Routes: Routes = [
  { path: "", redirectTo: "event-types", pathMatch: "full" },
  availability_Route,
  { path: "event-types", children: eventType_Routes },
  scheduled_Appointment_Routes,
  { path: "account-settings", children: accountSettings_Routes },
];

const auth_Routes: Routes = [
  { path: "", redirectTo: "login", pathMatch: "full" },
  { path: "login", component: LoginComponent }
];

const routes: Routes = [

  { path: "", component: AdminLayoutComponent, children: admin_Routes, canActivate: [AuthGuard] },
  { path: "auth", component: AuthLayoutComponent, children: auth_Routes },
  { path: "calendar", component: CalendarLayoutComponent, children: calendar_Routes }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
