import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventTypeComponent } from './event-type/eventtype.component';
import { FormsModule } from '@angular/forms';
import { Route, RouterModule } from '@angular/router';
import { EventInfoComponent } from './event-type/event-info/event-info.component';
import { EventAvailabilityComponent } from './event-type/event-availability/event-availability.component';
import { EventQuestionComponent } from './event-type/event-question/event-question.component';
import { EventTypeListComponent } from './event-type-list/event-type-list.component';
import { EventInfoNewComponent } from './event-type/event-info/event-info-new/event-info-new.component';
import { EventInfoUpdateComponent } from './event-type/event-info/event-info-update/event-info-update.component';
//import { CalenderComponent } from '../../controls/calender/calender.component';

//import { DailyTimeIntervalsComponent } from './event-type/event-availability/daily-time-intervals/daily-time-intervals.component';

const routes: Route[] = [
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
@NgModule({
    declarations: [
        // EventTypeListComponent,
        // EventTypeComponent,
        // EventInfoComponent,
        // EventAvailabilityComponent,
        // EventQuestionComponent,
        // EventInfoNewComponent,
        // EventInfoUpdateComponent,
        //CalenderComponent,
        //DailyTimeIntervalsComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild(routes),
    ]
})
export class EventTypeModule { }
