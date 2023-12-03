import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { IEventType } from 'projects/meetme/src/app/app-core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-event-info-new',
  templateUrl: './event-info-new.component.html',
  styleUrls: ['./event-info-new.component.scss']
})
export class EventInfoNewComponent implements OnInit, OnDestroy {
  destroyed$:Subject<boolean> = new Subject<boolean>();
  model: IEventType = {
    id: "",
    name: "",
    description: "",
    duration: 0,
    eventColor: "",
    ownerId: '',
    activeYN: true,
    location: '',
    slug: '',
    availabilityId: '',
    forwardDuration: 0,
    dateForwardKind: 'moving',
    bufferTimeAfter: 0,
    bufferTimeBefore: 0,
    timeZone: "",
    questions: []
  };
  constructor(private location: Location) { }

  ngOnInit(): void {
  }

  onSaved(response:any) {
    console.log(`Data saved ${response}`);
    this.location.back();
  }
  onCancelled() {
    this.location.back();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
