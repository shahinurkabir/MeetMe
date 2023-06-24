import { Location } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ICreateEventTypeCommand, IUpdateEventCommand } from 'projects/meetme/src/app/interfaces/event-type-commands';
import { IEventType } from 'projects/meetme/src/app/interfaces/event-type-interfaces';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';

@Component({
  selector: 'app-event-info-form',
  templateUrl: './event-info.component.html',
  styleUrls: ['./event-info.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventInfoComponent implements OnInit {
  submitted = false;
  @Input() model: IEventType = {
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
  @Output() dataSavedEvent = new EventEmitter();
  @Output() cancelEvent = new EventEmitter();

  eventColors = ["orange", "green", "blue", "cyan", "olive", "purple", "teal", "violet"]
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private location: Location
  ) {

  }
  onSelectedColor(event: any, color: string) {

    if (event)
      event.preventDefault()

    this.model.eventColor = color;
  }

  ngOnInit(): void {
  }

  onSubmit(form: any) {
    this.submitted = true;
    if (form.invalid) return;

    if (this.model.id != undefined && this.model.id.trim() !== "")
      this.handleUpdate();
    else
      this.handleAddNew();
  }

  private handleAddNew() {
    const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    let command: ICreateEventTypeCommand = {
      name: this.model.name,
      description: this.model.description,
      slug: this.model.slug,
      eventColor: this.model.eventColor,
      activeYN: false,
      timeZoneName: timezone
    };

    this.eventTypeService.addNew(command).subscribe({
      next: response => {
        this.saveComplete(response)
      },
      error: (error) => { console.log(error)}
    });
  }

  private handleUpdate() {
    let command: IUpdateEventCommand = {
      id: this.model.id,
      name: this.model.name,
      description: this.model.description,
      slug: this.model.slug,
      eventColor: this.model.eventColor,
    };

    this.eventTypeService.update(command).subscribe({
      next: response => {
        this.saveComplete(response)
      },
      error: (error) => { console.log(error) }
    });
  }

  private saveComplete(response: any) {
    this.dataSavedEvent?.emit(response)
  }
  onCancel() {
    this.cancelEvent?.emit()
  }
}
