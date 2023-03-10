import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { day_of_week, default_endTime_Minutes, default_meeting_buffertime, default_meeting_duration, default_meeting_forward_Duration_inDays, default_startTime_minutes, meeting_day_type_date, meeting_day_type_weekday, month_of_year } from 'projects/meetme/src/app/constants/default-data';
import { EventTypeAvailability, EventAvailabilityDetailItem, TimeZoneData } from 'projects/meetme/src/app/models/eventtype';
import { ListItem } from 'projects/meetme/src/app/models/list-item';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';
import { TimeZoneService } from 'projects/meetme/src/app/services/timezone.service';
import { convertToDays } from 'projects/meetme/src/app/utilities/functions';

@Component({
  selector: 'app-event-availability',
  templateUrl: './event-availability.component.html',
  styleUrls: ['./event-availability.component.css']
})
export class EventAvailabilityComponent implements OnInit {
  timeZoneList: TimeZoneData[] = [];
  eventTypeId: string = "";
  weeklyTimeAvailabilities: ITimeIntervalInDay[] = [];
  timeAvailability: EventTypeAvailability | undefined;
  model: model = this.getDefaultModel();
  meetingDurations: ListItem[] = [];
  selecteDateOverrideIntervals: ITimeIntervalInDay | undefined;
  dateOverrideAvailability: ITimeIntervalInDay[] = [];
  selectedDatesFromCalender: { [id: string]: string } = {};

  constructor(
    private eventTypeService: EventTypeService,
    private timeZoneService: TimeZoneService,
    private route: ActivatedRoute
  ) {

    this.subscribeParamentRouteParams();
    this.initMeetingDurationAndTypes();
    this.loadTimeZoneList();

  };

  subscribeParamentRouteParams() {
    this.route.parent?.params.subscribe((params) => {
      this.eventTypeId = params["id"];
      this.loadDataFromServer(this.eventTypeId);

    });
  }

  loadTimeZoneList() {
    this.timeZoneService.getList().subscribe(res => {
      this.timeZoneList = res;
    })
  }

  ngOnInit(): void {
  }

  onAddTimeInterval(timeInteralsInDay: ITimeIntervalInDay) {
    let startTime_Minutes = default_startTime_minutes;
    let endTime_Minutes = default_endTime_Minutes;

    let intervals = timeInteralsInDay.intervals;

    if (intervals.length > 0) {
      let lastSlot = intervals[intervals.length - 1];
      startTime_Minutes = (lastSlot.endTimeInMinute + 60);
      endTime_Minutes = startTime_Minutes + 60;

      if (startTime_Minutes >= 1440)// changing pm to am
      {
        startTime_Minutes = 0;
        endTime_Minutes = 60;
      }

    }

    let timeSlot = this.getTimeInterval(startTime_Minutes, endTime_Minutes);

    intervals.push(timeSlot)

    timeInteralsInDay.isAvailable = true;

    this.validateOverlapIntervals(timeInteralsInDay);
  }

  onRemoveTimeInterval(index: number, dailyTimeAvailabilities: ITimeIntervalInDay) {

    dailyTimeAvailabilities.intervals.splice(index, 1);

    if (dailyTimeAvailabilities.intervals.length == 0)
      dailyTimeAvailabilities.isAvailable = false;

    this.validateOverlapIntervals(dailyTimeAvailabilities);

  }
 
  loadDataFromServer(id: string) {
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
      this.model.forwardDurationInDays = convertToDays(this.timeAvailability.forwardDuration)
    }

    this.updateTimeIntervalData();
  }

  updateTimeIntervalData() {

    this.resetTimeIntervals();

    let timeAvailabilityInWeek = this.timeAvailability?.availabilityDetails
      .filter(e => e.type === meeting_day_type_weekday);


    timeAvailabilityInWeek?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.weeklyTimeAvailabilities.find(e => e.day == item.day);
      dailyTimeAvailabilities?.intervals.push(intervalItem)
    })

    let timeOverridesAvailability = this.timeAvailability?.availabilityDetails
      .filter(e => e.type === meeting_day_type_date);

    timeOverridesAvailability?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.dateOverrideAvailability.find(e => e.day == item.date);
      if (!dailyTimeAvailabilities) {
        dailyTimeAvailabilities = { day: item.date!, isAvailable: true, intervals: [] };
        this.dateOverrideAvailability.push(dailyTimeAvailabilities);
      }
      dailyTimeAvailabilities?.intervals.push(intervalItem);
    })
  }

  
  onLostFocus(e: Event, index: number, isEndTime: boolean, dailyTimeAvailabilities: ITimeIntervalInDay) {
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
    // overrides intervals
    for (let timeIntervalInDate of this.dateOverrideAvailability) {
      for (let timeInterval of timeIntervalInDate.intervals) {
        timeAvalabilityDetails.push(
          {
            type: "date",
            stepId: 0,
            date: timeIntervalInDate.day,
            from: timeInterval.startTimeInMinute,
            to: timeInterval.endTimeInMinute,
          });
      }
    }

    let forwardDuration = parseInt(this.model.forwardDurationInDays.toString()) * (24 * 60);

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
  
  onToggleCalendarModal(event: any) {
    event.preventDefault();
    this.toggleModalBackDrop();
    this.toggleOverrideModal();
    this.toggleBodyScrollY();

    this.selecteDateOverrideIntervals = undefined;
    this.selectedDatesFromCalender = {};

  }

  onApplyCalendarDateChanges(event: any) {

    event.preventDefault();

    for (let date in this.selectedDatesFromCalender) {

      let index = this.dateOverrideAvailability.findIndex(e => e.day == date);

      if (index > -1) this.dateOverrideAvailability.splice(index, 1);

      if (this.selecteDateOverrideIntervals?.intervals.length! > 0) {
        let timeIntervalInDay: ITimeIntervalInDay = { day: date, isAvailable: true, intervals: [] };

        this.selecteDateOverrideIntervals?.intervals.forEach(interval => {
          timeIntervalInDay.intervals.push(interval);
        })
        this.dateOverrideAvailability.push(timeIntervalInDay);
      }
    }

    this.dateOverrideAvailability.sort((a, b) => new Date(a.day).getTime() - new Date(b.day).getTime());

    this.onToggleCalendarModal(event);

  }
  
  removeOverrideData(index: number) {
    this.dateOverrideAvailability.splice(index, 1);
  }
  
  onDateClicked(selectedDates: { [id: string]: string }) {

    //this.selectedDatesFromCalender = selectedDates
    if (this.selecteDateOverrideIntervals) return;

    let timeSlot = this.getTimeInterval(default_startTime_minutes, default_endTime_Minutes)
    this.selecteDateOverrideIntervals = { day: '', isAvailable: true, intervals: [] }
    this.selecteDateOverrideIntervals.intervals.push(timeSlot);
  }

  toggleBodyScrollY() {
    document.body.classList.toggle('is-modal-open')
  }
  
  toggleModalBackDrop() {
    document.querySelector('#modal-backdrop')?.classList.toggle('is-open')
  }

  private getTimeInterval(startTime_Minutes: number, endTime_Minutes: number): ITimeInterval {
    let timeSlot: ITimeInterval = {
      startTime: this.convertTime(startTime_Minutes),
      startTimeInMinute: startTime_Minutes,
      endTime: this.convertTime(endTime_Minutes),
      endTimeInMinute: endTime_Minutes
    };
    return timeSlot;
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

  private validateOverlapIntervals(timeIntervalsInDay: ITimeIntervalInDay) {

    // clean error message
    timeIntervalsInDay.intervals.forEach(e => e.errorMessage = "");

    let intervals = timeIntervalsInDay.intervals;
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

  // private convertMinutes(time: string): number {
  //   let timeParts = time.split(":");
  //   let isPM = timeParts[1].indexOf("pm") != -1;
  //   let hours = parseInt(timeParts[0]);
  //   if (isPM)
  //     hours = hours + 12;
  //   else if (hours == 12)
  //     hours = 0
  //   let minutesParts = timeParts[1].replace("am", "").replace("pm", "");
  //   let minutes = parseInt(minutesParts);
  //   minutes = (hours * 60) + minutes;

  //   return minutes;
  // }

  private resetTimeIntervals() {
    this.dateOverrideAvailability = [];
    this.weeklyTimeAvailabilities = [];
    day_of_week.forEach(weekDay => {
      let dailyTimeAvailabilities: ITimeIntervalInDay = {
        day: weekDay, isAvailable: true, intervals: []
      }
      this.weeklyTimeAvailabilities.push(dailyTimeAvailabilities);
    })
  }

  private getDefaultModel(): model {

    let item: model = {
      id: "",
      meetingDuration: default_meeting_duration,
      dateForwardKind: "Moving",
      forwardDurationInDays: default_meeting_forward_Duration_inDays,
      bufferTimeAfter: default_meeting_buffertime,
      bufferTimeBefore: default_meeting_buffertime,
      timeZoneId: 1,
      availabilityDetails: [],
    };
    return item;
  }
  private initMeetingDurationAndTypes() {
    this.meetingDurations.push({ text: "15 min", value: "15" });
    this.meetingDurations.push({ text: "30 min", value: "30" });
    this.meetingDurations.push({ text: "45 min", value: "45" });
    this.meetingDurations.push({ text: "60 min", value: "60" });

  }

}
export interface ITimeIntervalInDay {
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
