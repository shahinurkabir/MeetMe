import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TimeZoneData, IUpdateEventAvailabilityCommand, ListItem, IAvailability, IEventAvailabilityDetailItemDto, IEventType, TimeAvailabilityComponent, EventTypeService, AvailabilityService, settings_day_of_week, IAvailabilityDetails, settings_meeting_day_type_weekday, settings_starttime_minutes, settings_endtime_minutes, AlertService, CommonFunction, setting_meetting_forward_Duration_kind } from 'projects/meetme/src/app/app-core';
import { Observable, Subject, forkJoin, takeUntil } from 'rxjs';

@Component({
  selector: 'app-event-availability',
  templateUrl: './event-availability.component.html',
  styleUrls: ['./event-availability.component.css']
})
export class EventAvailabilityComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  timeZoneList: TimeZoneData[] = [];
  eventTypeId: string = "";
  eventTypeAvailability: IUpdateEventAvailabilityCommand | undefined;
  // meetingDurations: ListItem[] = [];
  selectedDatesFromCalender: { [id: string]: string } = {};
  isCustomAvailability: boolean = false;
  listOfAvailability: IAvailability[] = [];
  selectedAvailability: IAvailability | undefined
  eventAvailabilityDetails: IEventAvailabilityDetailItemDto[] = [];
  customAvailability: IAvailability | undefined;
  isLoading: boolean = false;
  isValidTimeValidablitityCustom:boolean=true;
  submitted = false;
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
    forwardDurationInDays: 0,
    dateForwardKind: 'moving',
    bufferTimeAfter: 0,
    bufferTimeBefore: 0,
    timeZone: "",
    questions: []
  };
  forwardDurationInDays: number = 30;

  @ViewChild("timeAvailabilityControlCustom", { static: true }) timeAvailabilityControlCustom: TimeAvailabilityComponent | undefined;
  @ViewChild("timeAvailabilityControl", { static: true }) timeAvailabilityControl: TimeAvailabilityComponent | undefined;
  @ViewChild("formAvailability") formAvailability: NgForm | undefined;
  constructor(
    private eventTypeService: EventTypeService,
    private availabilityService: AvailabilityService,
    private route: ActivatedRoute,
    private alertService: AlertService
  ) {

    //this.initMeetingDurationAndTypes();
    this.subscribeRouteParams();

  };

  ngOnInit(): void {

  }

  onCancel(e: any) {
    e.preventDefault();
    this.loadData().subscribe(responses => {
      this.loadDataCompleted(responses);
    });
  }
  subscribeRouteParams() {
    this.route.parent?.params
      .pipe(takeUntil(this.destroyed$))
      .subscribe((params) => {
        this.eventTypeId = params["id"];

        this.loadData()
          .pipe(takeUntil(this.destroyed$))
          .subscribe({
            next: responses => {
              this.loadDataCompleted(responses);
            },
            error: (error) => { console.log(error) },
            complete: () => { }
          });

      });
  };

  loadData(): Observable<[IAvailability[], IEventType, IEventAvailabilityDetailItemDto[]]> {

    return forkJoin([
      this.availabilityService.getList(),
      this.eventTypeService.getById(this.eventTypeId),
      this.eventTypeService.getEventAvailability(this.eventTypeId)
    ]);
  }

  showTimeAvailabilityControl() {
    if (this.isCustomAvailability) {
      this.timeAvailabilityControlCustom?.setAvailability(this.customAvailability)
    } else {
      this.timeAvailabilityControl?.setAvailability(this.selectedAvailability)
    }
  }

  loadDataCompleted(responses: [IAvailability[], IEventType, IEventAvailabilityDetailItemDto[]]) {
    this.listOfAvailability = responses[0];
    this.model = responses[1];
    this.eventAvailabilityDetails = responses[2];

    this.customAvailability = this.convertEventScheduleToAvailabilitySchedule(this.model.timeZone, this.eventAvailabilityDetails);

    this.forwardDurationInDays = CommonFunction.convertToDays(this.model.forwardDurationInDays);

    

    if (this.model.availabilityId != null) {
      this.isCustomAvailability = false;
      this.selectedAvailability = this.listOfAvailability
        .find(e => e.id == this.model.availabilityId);
    }else {
      this.isCustomAvailability = true;
    }

    this.showTimeAvailabilityControl();
  }

  convertEventScheduleToAvailabilitySchedule(timeZone: string, eventTimeAvailabilityDetails?: IEventAvailabilityDetailItemDto[]): IAvailability {
    let availability: IAvailability = {
      id: '',
      name: "",
      ownerId: "",
      timeZone: timeZone,
      isDefault: false,
      details: [],
    };

    if (eventTimeAvailabilityDetails == undefined) {
      settings_day_of_week.forEach(day => {
        let availabilityDetailItem: IAvailabilityDetails = {
          dayType: settings_meeting_day_type_weekday,
          value: day,
          from: settings_starttime_minutes,
          to: settings_endtime_minutes,
          stepId: 0
        }
        availability.details.push(availabilityDetailItem);
      });
    }
    else {
      eventTimeAvailabilityDetails.forEach(detail => {
        let availabilityDetailItem: IAvailabilityDetails = {
          dayType: detail.dayType,
          value: detail.value,
          from: detail.from,
          to: detail.to,
          stepId: detail.stepId
        }
        availability.details.push(availabilityDetailItem);
      });
    }
    return availability;
  }

  onTimeAvailabilityChanged(isValidChanged: boolean) {
    this.isValidTimeValidablitityCustom=isValidChanged
  }
  onSubmit(e: any) {
    this.submitted = true;
    e.preventDefault();

    if (this.formAvailability!.invalid) return;
    if (this.isCustomAvailability && !this.isValidTimeValidablitityCustom) return ;
    
    let availability: IAvailability | undefined;

    if (this.isCustomAvailability) {
      availability = this.timeAvailabilityControlCustom?.getAvailability();
    }
    else {
      availability = this.timeAvailabilityControl?.getAvailability();
    }

    if (availability == undefined) return;

    let availabilityDetails: IEventAvailabilityDetailItemDto[] = [];

    availability.details.forEach(detail => {
      availabilityDetails.push({ dayType: detail.dayType, value: detail.value, from: detail.from, to: detail.to, stepId: detail.stepId });
    });

    let eventTypeAvailability: IUpdateEventAvailabilityCommand = {
      id: this.eventTypeId,
      dateForwardKind: setting_meetting_forward_Duration_kind.moving,
      forwardDurationInDays: this.forwardDurationInDays * 24 * 60,
      bufferTimeBefore: this.model.bufferTimeBefore,
      bufferTimeAfter: this.model.bufferTimeAfter,
      timeZone: this.model.timeZone,
      availabilityId: this.isCustomAvailability
        ? undefined : this.selectedAvailability?.id,
      availabilityDetails: availabilityDetails!
    };

    this.isLoading = true;
    this.eventTypeService.updateAvailability(eventTypeAvailability)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Availability updated successfully");

        },
        error: (error) => { CommonFunction.getErrorListAndShowIncorrectControls(this.formAvailability!, error.error.errors) },
        complete: () => { this.isLoading = false; }
      });

  }


  onClickExistingHours(e: any) {
    this.isCustomAvailability = false;
    this.showTimeAvailabilityControl();
    e.preventDefault();
  }
  onClickCustomHour(e: any) {
    this.isCustomAvailability = true;
    this.showTimeAvailabilityControl();
    e.preventDefault();
  }

  onChangeAvailability() {
    this.isCustomAvailability=this.selectedAvailability==undefined
    this.showTimeAvailabilityControl();
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}


