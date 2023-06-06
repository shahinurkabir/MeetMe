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
    id: "", name: "",
    description: "",
    eventColor: "",
    ownerId: '',
    activeYN: true,
    location: '',
    slug: '',
    availabilityId:''
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
