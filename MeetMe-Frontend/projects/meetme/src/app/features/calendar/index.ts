import { Routes } from "@angular/router";
import { EventTypeCalendarComponent } from "./eventtype-calendar/eventtype-calendar.component";
import { UserCalendarListComponent } from "./user-calendar-list/user-calendar-list.component";

export const calendar_Routes: Routes = [
    { path: ":user", component: UserCalendarListComponent },
    { path: ":user/:slug", component: EventTypeCalendarComponent },
    { path: "cancel/:id", component: EventTypeCalendarComponent },
    { path: "reschedule/:id", component: EventTypeCalendarComponent },
]

export const calendar_Components = [
    EventTypeCalendarComponent,
    UserCalendarListComponent
];
