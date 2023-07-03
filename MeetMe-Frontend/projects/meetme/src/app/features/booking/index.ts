import { Routes } from "@angular/router";
import { EventTypeCalendarComponent } from "./eventtype-calendar/eventtype-calendar.component";
import { UserCalendarComponent } from "./user-calendar/user-calendar.component";

export const booking_Routes: Routes = [
    { path: ":user", component: UserCalendarComponent },
    { path: ":user/:slug", component: EventTypeCalendarComponent },
    { path: "cancel/:id", component: EventTypeCalendarComponent },
    { path: "reschedule/:id", component: EventTypeCalendarComponent },
]

export const booking_Components = [
    EventTypeCalendarComponent,
    UserCalendarComponent
];
