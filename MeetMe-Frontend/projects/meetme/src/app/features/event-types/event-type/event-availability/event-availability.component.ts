import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, EventType } from '@angular/router';
import { day_of_week, default_endTime_Minutes, default_meeting_buffertime, default_meeting_duration, default_meeting_forward_Duration_inDays, default_startTime_minutes, meeting_day_type_date, meeting_day_type_weekday, month_of_year } from 'projects/meetme/src/app/constants/default-data';
import { TimeAvailabilityComponent } from 'projects/meetme/src/app/controls/time-availability/time-availability.component';
import { IAvailability, IAvailabilityDetails } from 'projects/meetme/src/app/interfaces/availability-interfaces';
import { IUpdateEventAvailabilityCommand } from 'projects/meetme/src/app/interfaces/event-type-commands';
import { TimeZoneData, EventAvailabilityDetailItemDto, IEventType } from 'projects/meetme/src/app/interfaces/event-type-interfaces';
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
  isExistingHours: boolean = true;
  listOfAvailability: IAvailability[] = [];
  selectedAvailability: IAvailability | undefined
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
    timeZone: ""
  };
  forwardDurationInDays: number = 30;

  @ViewChild("availabilityExistingComponent", { static: true }) timeAvailabilityExisting: TimeAvailabilityComponent | undefined;
  @ViewChild("availabilityComponent", { static: true }) timeAvailabilityComponent: TimeAvailabilityComponent | undefined;
  constructor(
    private eventTypeService: EventTypeService,
    private availabilityService: AvailabilityService,
    private route: ActivatedRoute
  ) {

    this.initMeetingDurationAndTypes();
    this.createCustomAvailabilityModel();
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

  loadData(eventTypeId: string): Observable<[IAvailability[], IEventType]> {

    return forkJoin([this.availabilityService.getList(),
    this.eventTypeService.getById(this.eventTypeId)]);
  }

  initTimeAvailabilityComponent() {
    if (this.isExistingHours) {
      this.timeAvailabilityExisting?.setAvailability(this.selectedAvailability)
    } else {
      this.timeAvailabilityComponent?.setAvailability(this.customAvailability)
    }
  }

  loadDataCompleted(responses: [IAvailability[], IEventType]) {
    this.listOfAvailability = responses[0];
    this.model = responses[1];;

    if (this.model.forwardDuration != null) {
      this.forwardDurationInDays = convertToDays(this.model.forwardDuration);
    }
    if (this.model.availabilityId == null) {
      this.isExistingHours = false;
    }
    else {
      this.isExistingHours = true;
      this.selectedAvailability = this.listOfAvailability.find(e => e.id == this.model.availabilityId);
    }
    this.initTimeAvailabilityComponent();
  }

  createCustomAvailabilityModel() {
    let availability: IAvailability = {
      id: '',
      name: "",
      ownerId: "",
      timeZone: "",
      isDefault: false,
      details: [],
      isCustom: true
    };

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

    this.customAvailability = availability;

  }

  onSubmit(form: any) {

    if (form.invalid) return;

    let availability: IAvailability | undefined;

    if (this.isExistingHours) {
      availability = this.timeAvailabilityExisting?.getAvailability();
    }
    else {
      availability = this.timeAvailabilityComponent?.getAvailability();
    }

    if (availability == undefined) return;

    let availabilityDetails: EventAvailabilityDetailItemDto[] = [];

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
      availabilityId: this.isExistingHours?this.selectedAvailability?.id:undefined,
      availabilityDetails: availabilityDetails!
    };

    this.eventTypeService.updateAvailability(eventTypeAvailability).subscribe(response => {
      alert("Data saved successfully.")
    });

  }


  // private getDefaultModel(): model {

  //   let item: model = {
  //     id: "",
  //     meetingDuration: default_meeting_duration,
  //     dateForwardKind: "Moving",
  //     forwardDurationInDays: default_meeting_forward_Duration_inDays,
  //     bufferTimeAfter: default_meeting_buffertime,
  //     bufferTimeBefore: default_meeting_buffertime,
  //     timeZoneId: 1,
  //     availabilityDetails: [],
  //   };
  //   return item;
  // }
  onClickExistingHours(e: any) {
    this.isExistingHours = true;
    e.preventDefault();
  }
  onClickCustomHour(e: any) {
    this.isExistingHours = false;
    this.initTimeAvailabilityComponent();
    e.preventDefault();
  }

  onChangeAvailability() {
    this.initTimeAvailabilityComponent();
  }

  private initMeetingDurationAndTypes() {
    this.meetingDurations.push({ text: "15 min", value: "15" });
    this.meetingDurations.push({ text: "30 min", value: "30" });
    this.meetingDurations.push({ text: "45 min", value: "45" });
    this.meetingDurations.push({ text: "60 min", value: "60" });

  }

}

interface model {
  id: string,
  meetingDuration: number,
  dateForwardKind: string,
  forwardDurationInDays: number,
  bufferTimeAfter: number,
  bufferTimeBefore: number,
  timeZone: string,
  availabilityId?: string,
  availabilityDetails: [],
}
