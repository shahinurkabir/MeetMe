import { Location } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IEventType, EventTypeService, ICreateEventTypeCommand, IUpdateEventCommand, AlertService, CommonFunction } from 'projects/meetme/src/app/app-core';
import { Subject, takeUntil } from 'rxjs';
import { DataExchangeService } from '../../services/data-exchange-services';

@Component({
  selector: 'app-event-info-form',
  templateUrl: './event-info.component.html',
  styleUrls: ['./event-info.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EventInfoComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  submitted = false;
  @Input() eventTypeId: string | undefined;

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
    questions: [],
  };
  @Output() dataSavedEvent = new EventEmitter();
  @Output() cancelEvent = new EventEmitter();

  eventColors = ["orange", "green", "blue", "cyan", "olive", "purple", "teal", "violet"]
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private location: Location,
    private alertServie: AlertService,
    private dataExchangeService: DataExchangeService
  ) {

  }
  ngOnInit(): void {
    if (this.eventTypeId !== undefined)
      this.loadEventTypeDetail(this.eventTypeId);
  }

  onSubmit(form: any) {
    this.submitted = true;
    if (form.invalid) return;

    if (this.model.id != undefined && this.model.id.trim() !== "")
      this.handleUpdate(form);
    else
      this.handleAddNew(form);
  }

  onSelectedColor(event: any, color: string) {

    if (event)
      event.preventDefault()

    this.model.eventColor = color;
  }


  loadEventTypeDetail(eventTypeId: string) {
    this.eventTypeService.getById(eventTypeId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.model = response;
          this.dataExchangeService.setEventTypeTitle(response.name);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })

  }


  private handleAddNew(form: any) {
    const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    let command: ICreateEventTypeCommand = {
      name: this.model.name,
      description: this.model.description,
      slug: this.model.slug,
      eventColor: this.model.eventColor,
      activeYN: false,
      timeZoneName: timezone
    };

    this.eventTypeService.addNew(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertServie.success("New event type has been created.");
          this.saveComplete(response)
        },
        error: (error) => { CommonFunction.getErrorListAndShowIncorrectControls(form, error.error.errors); },
        complete: () => { }
      });
  }

  private handleUpdate(form: any) {
    let command: IUpdateEventCommand = {
      id: this.model.id,
      name: this.model.name,
      description: this.model.description,
      slug: this.model.slug,
      eventColor: this.model.eventColor,
    };

    this.eventTypeService.update(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertServie.success("Event type has been updated.");
          this.saveComplete(response)
        },
        error: (error) => { CommonFunction.getErrorListAndShowIncorrectControls(form, error.error.errors) },
        complete: () => { }
      });
  }

  private saveComplete(response: any) {
    this.dataSavedEvent?.emit(response)
  }
  onCancel() {
    this.cancelEvent?.emit()
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }

}
