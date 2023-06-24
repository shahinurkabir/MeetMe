import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AvailabilityComponent } from './features/availability/availability.component';
import { EventTypeListComponent } from './features/event-types/event-type-list/event-type-list.component';
import { EventAvailabilityComponent } from './features/event-types/event-type/event-availability/event-availability.component';
import { EventInfoNewComponent } from './features/event-types/event-type/event-info/event-info-new/event-info-new.component';
import { EventInfoUpdateComponent } from './features/event-types/event-type/event-info/event-info-update/event-info-update.component';
import { EventQuestionComponent } from './features/event-types/event-type/event-question/event-question.component';
import { EventTypeComponent } from './features/event-types/event-type/eventtype.component';
import { LoginComponent } from './features/users/login/login.component';
//import { WorkinghoursComponent } from './features/workinghours/workinghours.component';
import { AuthGuard } from './gurads/auth-gurad';
import { HomeComponent } from './home/home.component';
import { CalendarComponent } from './controls/calender/calendar.component';
import { EventTypeCalendarComponent } from './features/eventtype-calendar/eventtype-calendar.component';
import { TestComponentComponent } from './test-component/test-component.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { BookingLayoutComponent } from './layouts/booking-layout/booking-layout.component';

const admin_Routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "availability", component: AvailabilityComponent },
  {
    path: "event-types", children: [
      { path: "", component: EventTypeListComponent },
      { path: "new", component: EventInfoNewComponent },
      {
        path: ":id", component: EventTypeComponent,
        children: [
          { path: "", pathMatch: "full", redirectTo: "info" },
          { path: "info", component: EventInfoUpdateComponent },
          { path: "availability", component: EventAvailabilityComponent },
          { path: "question", component: EventQuestionComponent }
        ]
      }

    ]
  },
  { path: "account-settings", loadChildren: () => import("./features/acccount-settings/account-settings.module").then(m => m.AccountSettingsModule) },

  { path: "test", component: TestComponentComponent }

];
const auth_Routes: Routes = [
  { path: "", redirectTo: "login", pathMatch: "full" },
  { path: "login", component: LoginComponent }
];

const booking_Routes: Routes = [
  { path: "appointment/:id/:slug", component: EventTypeCalendarComponent }
]
const listRoute: Routes = [

  { path: "", component: AdminLayoutComponent, children: admin_Routes, canActivate: [AuthGuard] },
  { path: "auth", component: AuthLayoutComponent, children: auth_Routes },
  { path: "booking", component: BookingLayoutComponent, children: booking_Routes }
];

const routes: Routes = [
  // {
  //   path: "", component: HomeComponent,
  //   canActivate: [AuthGuard]
  // },
  {
    path: "", redirectTo: "event-types", pathMatch: "full"
  },
  { path: "login", component: LoginComponent },
  { path: 'availability', component: AvailabilityComponent, canActivate: [AuthGuard] },
  //{ path: "event-types", loadChildren: () => import("./features/event-types/event-type.module").then(m => m.EventTypeModule), canActivate: [AuthGuard] },
  // { path: "working-hours", component: WorkinghoursComponent, canActivate: [AuthGuard] },
  {
    path: "event-types", children: [
      { path: "", component: EventTypeListComponent },
      { path: "new", component: EventInfoNewComponent },
      {
        path: ":id", component: EventTypeComponent,
        children: [
          { path: "", pathMatch: "full", redirectTo: "info" },
          { path: "info", component: EventInfoUpdateComponent },
          { path: "availability", component: EventAvailabilityComponent },
          { path: "question", component: EventQuestionComponent }
        ]
      }

    ]
  }
  ,
  { path: "account-settings", loadChildren: () => import("./features/acccount-settings/account-settings.module").then(m => m.AccountSettingsModule), canActivate: [AuthGuard] },
  { path: "appointment/:id/:slug", component: EventTypeCalendarComponent, canActivate: [AuthGuard] },
  { path: "test", component: TestComponentComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(listRoute)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
