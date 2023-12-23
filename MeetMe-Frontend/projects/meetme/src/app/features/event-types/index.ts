import { Route } from "@angular/router";
import { EventTypeListComponent } from "./event-type-list/event-type-list.component";
import { EventAvailabilityComponent } from "./event-type/event-availability/event-availability.component";
import { EventInfoUpdateComponent } from "./event-type/event-info-update/event-info-update.component";
import { EventQuestionComponent } from "./event-type/event-question/event-question.component";
import { EventTypeShellComponent } from "./event-type/event-type-shell.component";
import { EventInfoModalComponent } from "./event-type/event-info-modal.component/event-info-modal.component";

export const eventType_Routes: Route[] =  [
  { path: "", component: EventTypeListComponent },
  {
    path: ":id", component: EventTypeShellComponent,
    children: [
      { path: "", pathMatch: "full", redirectTo: "info" },
      { path: "info", component: EventInfoUpdateComponent },
      { path: "availability", component: EventAvailabilityComponent },
      { path: "question", component: EventQuestionComponent }
    ]
  }
];

export const eventType_Components = [
    EventTypeListComponent,
    EventTypeShellComponent,
    EventInfoModalComponent,
    EventInfoUpdateComponent,
    EventAvailabilityComponent,
    EventQuestionComponent
];
        