import { Route } from "@angular/router";
import { AppointmentListComponent } from "./appointment-list/appointment-list.component";

export const scheduled_Appointment_Routes: Route =
    { path: "scheduled-appointments", component: AppointmentListComponent }


export const scheduled_Appointment_Components = [
    AppointmentListComponent
];
