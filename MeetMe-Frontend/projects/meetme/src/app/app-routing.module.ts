import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './features/users/login/login.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './app-core';
import { CalendarLayoutComponent } from './layouts/calendar-layout/calendar-layout.component';

const admin_features_routes: Routes = [
  { path: "", redirectTo: "event-types", pathMatch: "full" },
  { path: "event-types", loadChildren: () => import('./features/event-types/event-type.module').then(m => m.EventTypeModule), canActivate: [AuthGuard] },
  { path: "availability", loadChildren: () => import('./features/availability/availability.module').then(m => m.AvailabilityModule), canActivate: [AuthGuard] },
  { path: "scheduled-appointments", loadChildren: () => import('./features/scheduled-appointments/schedule-events.module').then(m => m.ScheduleEventsModule), canActivate: [AuthGuard] },
  { path: "account-settings", loadChildren: () => import('./features/acccount-settings/account-settings.module').then(m => m.AccountSettingsModule), canActivate: [AuthGuard] },
];

const calendar_features_routes: Routes = [
  { path: '', loadChildren: () => import('./features/calendar/calendar.module').then(m => m.CalendarModule) }
];

const auth_Routes: Routes = [
  { path: "", redirectTo: "login", pathMatch: "full" },
  { path: "login", component: LoginComponent }
];

const routes: Routes = [

  { path: "", component: AdminLayoutComponent, children: admin_features_routes, canActivate: [AuthGuard] },
  { path: "auth", component: AuthLayoutComponent, children: auth_Routes },
  {
    path: "calendar", component: CalendarLayoutComponent, children: calendar_features_routes
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
