import { Route } from "@angular/router";
import { EventTypeListComponent } from "./event-type-list/event-type-list.component";
import { EventAvailabilityComponent } from "./event-type/event-availability/event-availability.component";
import { EventInfoNewComponent } from "./event-type/event-info/event-info-new/event-info-new.component";
import { EventInfoUpdateComponent } from "./event-type/event-info/event-info-update/event-info-update.component";
import { EventQuestionComponent } from "./event-type/event-question/event-question.component";
import { EventTypeComponent } from "./event-type/eventtype.component";
import { EventInfoComponent } from "./event-type/event-info/event-info.component";

export const eventType_Routes: Route[] =  [
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
];

export const eventType_Components = [
    EventTypeListComponent,
    EventTypeComponent,
    EventInfoComponent,
    EventInfoNewComponent,
    EventInfoUpdateComponent,
    EventAvailabilityComponent,
    EventQuestionComponent
];
        