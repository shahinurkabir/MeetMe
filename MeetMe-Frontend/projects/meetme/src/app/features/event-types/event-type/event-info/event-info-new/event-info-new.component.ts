import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { EventType } from 'projects/meetme/src/app/models/eventtype';

@Component({
  selector: 'app-event-info-new',
  templateUrl: './event-info-new.component.html',
  styleUrls: ['./event-info-new.component.scss']
})
export class EventInfoNewComponent implements OnInit {
  model: EventType = {
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
    timeZoneId: 1
  };
  constructor(private location: Location) { }

  ngOnInit(): void {
  }

  onSaved(response:any) {
    console.log(response);
  }
  onCancelled() {
    this.location.back();
  }
}
