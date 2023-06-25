import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, EventType } from '@angular/router';
import { day_of_week, default_endTime_Minutes, default_meeting_buffertime, default_meeting_duration, default_meeting_forward_Duration_inDays, default_startTime_minutes, meeting_day_type_date, meeting_day_type_weekday, month_of_year } from 'projects/meetme/src/app/constants/default-data';
import { TimeAvailabilityComponent } from 'projects/meetme/src/app/controls/time-availability/time-availability.component';
import { IAvailability, IAvailabilityDetails } from 'projects/meetme/src/app/interfaces/availability-interfaces';
import { IUpdateEventAvailabilityCommand } from 'projects/meetme/src/app/interfaces/event-type-commands';
import { TimeZoneData, IEventAvailabilityDetailItemDto, IEventType } from 'projects/meetme/src/app/interfaces/event-type-interfaces';
import { ListItem } from 'projects/meetme/src/app/interfaces/list-item';
import { AvailabilityService } from 'projects/meetme/src/app/services/availability.service';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';
//import { TimeZoneService } from 'projects/meetme/src/app/services/timezone.service';
import { convertToDays } from 'projects/meetme/src/app/utilities/functions';
import { ObjectUnsubscribedError, Observable, forkJoin } from 'rxjs';

@Component({
  selector: 'app-event-availability',
  templateUrl: './event-availability.component.html',
  styleUrls: ['./event-availability.component.css']
})
export class EventAvailabilityComponent implements OnInit {
  timeZoneList: TimeZoneData[] = [];
  eventTypeId: string = "";
  eventTypeAvailability: IUpdateEventAvailabilityCommand | undefined;
  //model: model = this.getDefaultModel();
  meetingDurations: ListItem[] = [];
  selectedDatesFromCalender: { [id: string]: string } = {};
  isCustomAvailability: boolean = false;
  listOfAvailability: IAvailability[] = [];
  selectedAvailability: IAvailability | undefined
  eventAvailabilityDetails: IEventAvailabilityDetailItemDto[] = [];
  customAvailability: IAvailability | undefined;

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
  forwardDurationInDays: number = 30;

  @ViewChild("timeAvailabilityControlCustom", { static: true }) timeAvailabilityControlCustom: TimeAvailabilityComponent | undefined;
  @ViewChild("timeAvailabilityControl", { static: true }) timeAvailabilityControl: TimeAvailabilityComponent | undefined;
  constructor(
    private eventTypeService: EventTypeService,
    private availabilityService: AvailabilityService,
    private route: ActivatedRoute
  ) {

    this.initMeetingDurationAndTypes();
    this.subscribeRouteParams();

  };

  ngOnInit(): void {

  }

  subscribeRouteParams() {
    this.route.parent?.params.subscribe((params) => {
      this.eventTypeId = params["id"];

      this.loadData(this.eventTypeId).subscribe(responses => {
        this.loadDataCompleted(responses);
      });

    });
  };

  loadData(eventTypeId: string): Observable<[IAvailability[], IEventType, IEventAvailabilityDetailItemDto[]]> {

    return forkJoin([
      this.availabilityService.getList(),
      this.eventTypeService.getById(eventTypeId),
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

    if (this.model.forwardDuration != null) {
      this.forwardDurationInDays = convertToDays(this.model.forwardDuration);
    }
    if (this.model.availabilityId != null) {
      this.isCustomAvailability = false;
      this.selectedAvailability = this.listOfAvailability
        .find(e => e.id == this.model.availabilityId);
    }
    else {
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
      isCustom: true
    };

    if (eventTimeAvailabilityDetails == undefined) {
      day_of_week.forEach(day => {
        let availabilityDetailItem: IAvailabilityDetails = {
          dayType: meeting_day_type_weekday,
          value: day,
          from: default_startTime_minutes,
          to: default_endTime_Minutes,
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

  onSubmit(form: any) {

    if (form.invalid) return;

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
      duration: this.model.duration,
      dateForwardKind: "moving",
      forwardDuration: this.forwardDurationInDays * 24 * 60,
      bufferTimeBefore: this.model.bufferTimeBefore,
      bufferTimeAfter: this.model.bufferTimeAfter,
      timeZone: this.model.timeZone,
      availabilityId: this.isCustomAvailability
        ? undefined : this.selectedAvailability?.id,
      availabilityDetails: availabilityDetails!
    };

    this.eventTypeService.updateAvailability(eventTypeAvailability).subscribe(response => {
      alert("Data saved successfully.")
    });

  }


  onClickExistingHours(e: any) {
    this.isCustomAvailability = false;
    e.preventDefault();
  }
  onClickCustomHour(e: any) {
    this.isCustomAvailability = true;
    this.showTimeAvailabilityControl();
    e.preventDefault();
  }

  onChangeAvailability() {
    this.showTimeAvailabilityControl();
  }

  private initMeetingDurationAndTypes() {
    this.meetingDurations.push({ text: "15 min", value: "15" });
    this.meetingDurations.push({ text: "30 min", value: "30" });
    this.meetingDurations.push({ text: "45 min", value: "45" });
    this.meetingDurations.push({ text: "60 min", value: "60" });

  }

}

// interface model {
//   id: string,
//   meetingDuration: number,
//   dateForwardKind: string,
//   forwardDurationInDays: number,
//   bufferTimeAfter: number,
//   bufferTimeBefore: number,
//   timeZone: string,
//   availabilityId?: string,
//   availabilityDetails: [],
// }
