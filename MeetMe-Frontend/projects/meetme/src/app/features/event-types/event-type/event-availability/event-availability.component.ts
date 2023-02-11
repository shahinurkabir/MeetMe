import { WeekDay } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventTypeAvailability, EventAvailabilityDetailItem, TimeZoneData } from 'projects/meetme/src/app/models/eventtype';
import { ListItem } from 'projects/meetme/src/app/models/list-item';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';
import { TimeZoneService } from 'projects/meetme/src/app/services/timezone.service';

@Component({
  selector: 'app-event-availability',
  templateUrl: './event-availability.component.html',
  styleUrls: ['./event-availability.component.css']
})
export class EventAvailabilityComponent implements OnInit {
  timeZoneList: TimeZoneData[] = [];
  eventTypeId: string = "";
  weekdays: string[] = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
  weeklyTimeAvailabilities: IDailyTimeAvailabilities[] = [];
  timeAvailability: EventTypeAvailability | undefined;
  model: model = this.getDefaultModel();
  meetingDurations: ListItem[] = [];

  constructor(
    private eventTypeService: EventTypeService,
    private timeZoneService: TimeZoneService,
    private route: ActivatedRoute
  ) {

    this.initMeetingDurationAndTypes();
    this.loadTimeZoneList();
    this.route.parent?.params.subscribe((params) => {
      this.eventTypeId = params["id"];
      this.loadEventTypeAvailability(this.eventTypeId);

    });

  }
  loadTimeZoneList() {
    this.timeZoneService.getList().subscribe(res => {
      this.timeZoneList = res;
    })
  }
  ngOnInit(): void {
  }

  onAddTimeInterval(dailyTimeAvailabilities: IDailyTimeAvailabilities) {
    let startTime_Minutes = 60 * 9 // 9:00am 
    let endTime_Minutes = 60 * 17;// 5:00pm 

    let intervals = dailyTimeAvailabilities.intervals;

    if (intervals.length > 0) {
      let lastSlot = intervals[intervals.length - 1];
      startTime_Minutes = (lastSlot.endTimeInMinute + 60);
      endTime_Minutes = startTime_Minutes + 60;

      if (startTime_Minutes >= 1440)// need to change pm to am
      {
        startTime_Minutes = 0;
        endTime_Minutes = 60;
      }

    }

    let timeSlot: ITimeInterval = {
      startTime: this.convertTime(startTime_Minutes),
      startTimeInMinute: startTime_Minutes,
      endTime: this.convertTime(endTime_Minutes),
      endTimeInMinute: endTime_Minutes
    };

    intervals.push(timeSlot)

    dailyTimeAvailabilities.isAvailable = true;

    this.validateOverlapIntervals(dailyTimeAvailabilities);
  }

  onRemoveTimeInterval(index: number, dailyTimeAvailabilities: IDailyTimeAvailabilities) {

    dailyTimeAvailabilities.intervals.splice(index, 1);

    if (dailyTimeAvailabilities.intervals.length == 0)
      dailyTimeAvailabilities.isAvailable = false;

    this.validateOverlapIntervals(dailyTimeAvailabilities);

  }
  onMeetingDurationChanged($event: any) {

  }

  loadEventTypeAvailability(id: string) {
    this.eventTypeService.getEventAvailability(id).subscribe(response => {
      console.log(response);
      this.timeAvailability = response;
      this.loadDataCompleted();
    });
  }

  loadDataCompleted() {
    if (this.timeAvailability)
      this.model.meetingDuration = this.timeAvailability.duration;

    if (this.timeAvailability?.forwardDuration) {
      this.model.forwardDurationInDays = this.convertToDays(this.timeAvailability.forwardDuration)
    }

    this.updateTimeIntervalData();
  }

  updateTimeIntervalData() {


    this.resetTimeIntervals();

    let timeAvailabilityInWeek = this.timeAvailability?.availabilityDetails
      .filter(e => e.type === 'weekday');

    timeAvailabilityInWeek?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.weeklyTimeAvailabilities.find(e => e.day == item.day);
      dailyTimeAvailabilities?.intervals.push(intervalItem)
    })

  }

  convertToDays(minutes: number): number {
    if (minutes <= 0) return 0;
    return (minutes / 60) / 24;
  }
  convertToMinutes(days: number) {
    if (days <= 0) return 0;
    return days * 24;
  }
  onLostFocus(e: Event, index: number, isEndTime: boolean, dailyTimeAvailabilities: IDailyTimeAvailabilities) {
    let htmlElement = e.target as HTMLInputElement;
    let timeValue = htmlElement.value;
    let intervalItem = dailyTimeAvailabilities.intervals[index];
    if (timeValue === undefined || timeValue.trim() === '') {
      intervalItem.errorMessage = "Invalid time";
      return;
    }

    let timeParts = timeValue.split(":");
    let hours = 0;
    let minutes = 0;

    if (timeParts.length > 0) {
      hours = parseInt(timeParts[0]);
      let indexOfHourlyFormat
      let isAM = false;
      let isPM = false;
      if (timeParts.length > 1) {
        indexOfHourlyFormat = timeParts[1].indexOf('a');
        isAM = indexOfHourlyFormat != -1 ? true : false;
        indexOfHourlyFormat = timeParts[1].indexOf('p');
        isPM = indexOfHourlyFormat != -1 ? true : false;

        if (isAM) {
          minutes = parseInt(timeParts[1])
        }
        else if (isPM) {
          hours = hours > 12 ? hours : hours + 12;
          minutes = parseInt(timeParts[1])
        }
        else {
          minutes = parseInt(timeParts[1].substring(0, 2))
        }
      }
      else {
        hours = parseInt(timeParts[0]);
        isPM = timeParts[0].indexOf('p') != -1;
        hours = (isPM && hours < 12) ? hours + 12 : hours;
      }
    }

    if (hours < 1) hours = 12;
    if (hours > 23) hours = 23;
    let totalMinutes = (hours * 60) + minutes;
    let time = this.convertTime(totalMinutes)


    if (isEndTime) {
      intervalItem.endTimeInMinute = totalMinutes
      intervalItem.endTime = time;
    }
    else {
      intervalItem.startTimeInMinute = totalMinutes
      intervalItem.startTime = time;
    }

    if (intervalItem.endTimeInMinute < intervalItem.startTimeInMinute) {
      intervalItem.errorMessage = "End time should be after than start time."
      htmlElement.focus();
    }
    else {
      intervalItem.errorMessage = "";
    }

    this.validateOverlapIntervals(dailyTimeAvailabilities)

  }
  onSubmit(form: any) {

    if (form.invalid) return;

    let timeAvalabilityDetails: EventAvailabilityDetailItem[] = [];

    for (let dailyTimeAvailabilities of this.weeklyTimeAvailabilities) {
      //let dailyTimeAvailabilities = this.weeklyTimeAvailabilities[weekDay];

      for (let timeInterval of dailyTimeAvailabilities.intervals) {
        timeAvalabilityDetails.push(
          {
            type: "weekday",
            stepId: 0,
            day: dailyTimeAvailabilities.day,
            from: timeInterval.startTimeInMinute,
            to: timeInterval.endTimeInMinute,
          });
      }
    }

    let forwardDuration = parseInt(this.model.forwardDurationInDays.toString()) * (24 * 60);
    // if (this.model.meetingDuration !== "custom") {
    //   meetingDuration = parseInt(this.model.meetingDuration);
    // }
    // else {
    //   if (this.model.meetingDurationType === 'min') {
    //     meetingDuration = parseInt(this.model.meetingDurationCustom);
    //   }
    //   else { //hourly
    //     meetingDuration = parseInt(this.model.meetingDurationCustom) * 60;
    //   }
    // }

    // let durationKind = this.model.meetingDuration == 'custom' ? 'custom' : 'normal';

    let eventTypeAvailability: EventTypeAvailability = {
      id: this.eventTypeId,
      duration: this.model.meetingDuration,
      dateForwardKind: "moving",
      forwardDuration: forwardDuration,
      bufferTimeBefore: this.model.bufferTimeBefore,
      bufferTimeAfter: this.model.bufferTimeAfter,
      timeZoneId: this.model.timeZoneId,
      availabilityDetails: timeAvalabilityDetails
    };

    this.eventTypeService.updateAvailability(eventTypeAvailability).subscribe(response => {
      alert("Data saved successfully.")
    });

  }
  onShowCalendarWidget(event: any) {
    event.preventDefault();
    this.toggleModalBackDrop();
    this.toggleOverrideModal();
  }
  toggleModalBackDrop() {
    document.querySelector('#modal-backdrop')?.classList.toggle('is-open')
  }
  private toggleOverrideModal() {
    document.querySelector('.modal-override-dates')?.classList.toggle('is-open')
  }
  private getTimeIntervalItem(fromTimeInMinute: number, endTimeInMinute: number): ITimeInterval {
    let item: ITimeInterval = {
      startTimeInMinute: fromTimeInMinute,
      startTime: this.convertTime(fromTimeInMinute),
      endTime: this.convertTime(endTimeInMinute),
      endTimeInMinute: endTimeInMinute
    }
    return item;
  }

  private validateOverlapIntervals(dailyTimeAvailabilities: IDailyTimeAvailabilities) {

    // clean error message
    dailyTimeAvailabilities.intervals.forEach(e => e.errorMessage = "");

    let intervals = dailyTimeAvailabilities.intervals;
    for (let i = 0; i < intervals.length; i++) {
      let item = intervals[i];

      for (let j = i + 1; j < intervals.length; j++) {
        let item2 = intervals[j];
        if (
          (item.startTimeInMinute >= item2.startTimeInMinute && item.startTimeInMinute <= item2.endTimeInMinute) ||
          (item.endTimeInMinute >= item2.startTimeInMinute && item.endTimeInMinute <= item2.endTimeInMinute)
        ) {
          item.errorMessage = "Times overlaping with other intervals";
          item2.errorMessage = "Times overlaping with other intervals";
          break;
        }
      }
    }
  }

  private convertTime(minutes: number): string {
    let hours = parseInt((minutes / 60).toFixed(0));
    let minutesRemaining = minutes - (hours * 60);
    let ampmFormat = hours > 12 ? "pm" : "am";
    hours = hours == 0 ? 12 : hours;
    let hoursFormatted = hours > 12 ? hours - 12 : hours;
    let minutesFormatted = minutesRemaining < 10 ? "0" + minutesRemaining : minutesRemaining;
    let time = hoursFormatted + ":" + minutesFormatted + ampmFormat;

    return time;
  }

  private convertMinutes(time: string): number {
    let timeParts = time.split(":");
    let isPM = timeParts[1].indexOf("pm") != -1;
    let hours = parseInt(timeParts[0]);
    if (isPM)
      hours = hours + 12;
    else if (hours == 12)
      hours = 0
    let minutesParts = timeParts[1].replace("am", "").replace("pm", "");
    let minutes = parseInt(minutesParts);
    minutes = (hours * 60) + minutes;

    return minutes;
  }

  private resetTimeIntervals() {
    this.weeklyTimeAvailabilities = [];
    this.weekdays.forEach(weekDay => {
      let dailyTimeAvailabilities: IDailyTimeAvailabilities = {
        day: weekDay, isAvailable: true, intervals: []
      }
      this.weeklyTimeAvailabilities.push(dailyTimeAvailabilities);
    })
  }

  private getDefaultModel(): model {

    let item: model = {
      id: "",
      meetingDuration: 30,
      dateForwardKind: "Moving",
      forwardDurationInDays: 60 * 24 * 60,
      bufferTimeAfter: 15,
      bufferTimeBefore: 15,
      timeZoneId: 1,
      availabilityDetails: [],
    }
    return item;
  }
  private initMeetingDurationAndTypes() {
    this.meetingDurations.push({ text: "15 min", value: "15" });
    this.meetingDurations.push({ text: "30 min", value: "30" });
    this.meetingDurations.push({ text: "45 min", value: "45" });
    this.meetingDurations.push({ text: "60 min", value: "60" });

  }

}
export interface IDailyTimeAvailabilities {
  day: string,
  isAvailable: boolean
  intervals: ITimeInterval[]
}
export interface ITimeInterval {
  startTime: string,
  endTime: string
  startTimeInMinute: number,
  endTimeInMinute: number,
  errorMessage?: string
}
interface model {
  id: string,
  meetingDuration: number,
  dateForwardKind: string,
  forwardDurationInDays: number,
  bufferTimeAfter: number,
  bufferTimeBefore: number,
  timeZoneId: number,
  availabilityDetails: [],
}
