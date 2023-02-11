import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventType } from 'projects/meetme/src/app/models/eventtype';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';

@Component({
  selector: 'app-event-info-update',
  templateUrl: './event-info-update.component.html',
  styleUrls: ['./event-info-update.component.scss']
})
export class EventInfoUpdateComponent implements OnInit {
  model: EventType = {
    id: "", name: "",
    description: "",
    eventColor: "",
    ownerId: '',
    activeYN: true,
    location: '',
    slug: ''
  };
  constructor(private eventTypeService: EventTypeService,
    private route: ActivatedRoute) {

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
    console.log(response);
  }
  onCancelled() {
    //this.location.back();
  }

}
