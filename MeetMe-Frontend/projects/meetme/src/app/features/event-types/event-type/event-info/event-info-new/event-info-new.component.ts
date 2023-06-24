import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IEventType } from 'projects/meetme/src/app/interfaces/event-type-interfaces';

@Component({
  selector: 'app-event-info-new',
  templateUrl: './event-info-new.component.html',
  styleUrls: ['./event-info-new.component.scss']
})
export class EventInfoNewComponent implements OnInit {
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
}
