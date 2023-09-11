import { Routes } from "@angular/router";
import { EventTypeCalendarComponent } from "./eventtype-calendar/eventtype-calendar.component";
import { UserCalendarListComponent } from "./user-calendar-list/user-calendar-list.component";
import { AppointmentDetailComponent } from "./appointment-detail/appointment-detail.component";
import { AppointmentCancelComponent } from "./appointment-cancel/appointment-cancel.component";

export const calendar_Routes: Routes = [
    { path: ":user", component: UserCalendarListComponent },
    { path: ":user/:slug", component: EventTypeCalendarComponent },
    { path: "booking/:user/:slug/:id/view", component: AppointmentDetailComponent },
    { path: "booking/:user/:slug/:id/cancellation", component: AppointmentCancelComponent },
    { path: "booking/:user/:slug/:id/reschedule", component: EventTypeCalendarComponent },
]

export const calendar_Components = [
    EventTypeCalendarComponent,
    UserCalendarListComponent,
    AppointmentDetailComponent,
    EventTypeCalendarComponent
];
