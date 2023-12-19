import { Route } from "@angular/router";
import { AvailabilityComponent } from "./availability.component";
import { AvailabilityListComponent } from "./availability-list/availability-list.component";

export const availability_Routes: Route[] = [
    { path: "", component: AvailabilityComponent }
]

export const availability_Components = [
    AvailabilityListComponent,
    AvailabilityComponent
];