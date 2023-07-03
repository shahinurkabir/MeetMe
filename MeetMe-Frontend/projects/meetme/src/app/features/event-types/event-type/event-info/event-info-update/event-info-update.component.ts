import { Component, OnInit } from '@angular/core';
import  {Location} from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { IEventType, EventTypeService } from 'projects/meetme/src/app/app-core';

@Component({
  selector: 'app-event-info-update',
  templateUrl: './event-info-update.component.html',
  styleUrls: ['./event-info-update.component.scss']
})
export class EventInfoUpdateComponent implements OnInit {
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
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private location: Location
    ) {

    this.route.parent?.params.subscribe((params) => {

      let eventTypeId = params["id"];
      this.loadEventTypeDetail(eventTypeId);

    });

  }

  ngOnInit(): void {
  }

  loadEventTypeDetail(id: string) {
    this.eventTypeService.getById(id).subscribe(response => {
      this.model = response;
    })
  }
  onSaved(response: any) {
    console.log(`Data saved ${response}`);
    this.location.back();
  }
  onCancelled() {
    this.location.back();
  }

}
