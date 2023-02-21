import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { interval } from 'rxjs';
import { day_of_week, default_endTime_Minutes, default_startTime_minutes, meeting_day_type_date, meeting_day_type_weekday, month_of_year } from '../../constants/default-data';
import { EventAvailabilityDetailItem } from '../../models/eventtype';
import { ITimeInterval } from '../../models/ITimeInterval';
import { ITimeIntervalInDay } from '../../models/ITimeIntervalInDay';
import { ListItem } from '../../models/list-item';
import { toggleModalDialog } from '../../utilities/functions';
import { CalendarComponent } from '../calender/calendar.component';

@Component({
  selector: 'app-time-availability',
  templateUrl: './time-availability.component.html',
  styleUrls: ['./time-availability.component.scss']
})
export class TimeAvailabilityComponent implements OnInit, AfterViewInit{
  weekDays = day_of_week;
  monthNames = month_of_year
  meetingDurations: ListItem[] = [];
  availabilityInWeek: ITimeIntervalInDay[] = [];
  availabilityInMonth: ICalendarDay[] = [];
  availabilityOnDateOverrides: ITimeIntervalInDay[] = [];
  selecteDateOverrideIntervals: ITimeIntervalInDay | undefined;
  selectedDatesFromCalender: { [id: string]: string } = {};
  listAvailability: EventAvailabilityDetailItem[] = [];
  viewMode: string = "list";
  calendarModalEl: Element | null = null;
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";

  @ViewChild(CalendarComponent) calendarComponent!:CalendarComponent;
  constructor() {

  }
  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    this.calendarModalEl = document.querySelector('.modal-override-dates');
    let currentDate = new Date();
    this.selectedYear = currentDate.getFullYear();
    this.selectedMonth = currentDate.getMonth();
    this.updateTimeIntervalData();
    this.resetMonthlyViewData();
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

  updateTimeIntervalData() {

    this.resetTimeIntervals();

    let timeAvailabilityInWeek = this.listAvailability
      .filter(e => e.type === meeting_day_type_weekday);


    timeAvailabilityInWeek?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityInWeek.find(e => e.day == item.day);
      dailyTimeAvailabilities?.intervals.push(intervalItem)
    })

    let timeOverridesAvailability = this.listAvailability
      .filter(e => e.type === meeting_day_type_date);

    timeOverridesAvailability?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityOnDateOverrides.find(e => e.day == item.date);
      if (!dailyTimeAvailabilities) {
        dailyTimeAvailabilities = { day: item.date!, isAvailable: true, intervals: [] };
        this.availabilityOnDateOverrides.push(dailyTimeAvailabilities);
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



  onToggleCalendarModal(dateSelect?:number) {
    //event.preventDefault();

    toggleModalDialog(this.calendarModalEl);

    this.selecteDateOverrideIntervals = undefined;
    this.selectedDatesFromCalender = {};
    //this.selectedDatesFromCalender={};
    this.calendarComponent.resetSelection(dateSelect);
  }

  onApplyCalendarDateChanges() {

    //event.preventDefault();

    for (let date in this.selectedDatesFromCalender) {

      let index = this.availabilityOnDateOverrides.findIndex(e => e.day == date);

      if (index > -1) this.availabilityOnDateOverrides.splice(index, 1);

      if (this.selecteDateOverrideIntervals?.intervals.length! > 0) {
        let timeIntervalInDay: ITimeIntervalInDay = { day: date, isAvailable: true, intervals: [] };

        this.selecteDateOverrideIntervals?.intervals.forEach(interval => {
          timeIntervalInDay.intervals.push(interval);
        })
        this.availabilityOnDateOverrides.push(timeIntervalInDay);
      }
    }

    this.availabilityOnDateOverrides.sort((a, b) => new Date(a.day).getTime() - new Date(b.day).getTime());

    this.onToggleCalendarModal();

  }

  removeOverrideData(index: number) {
    this.availabilityOnDateOverrides.splice(index, 1);
  }
  editOverrideData(index: number) {
    this.selecteDateOverrideIntervals = this.availabilityOnDateOverrides[index];
    let daySelect=new Date(this.selecteDateOverrideIntervals.day).getDate();
    this.selecteDateOverrideIntervals=undefined;
    this.onToggleCalendarModal(daySelect);
  }
  onDateClicked(selectedDates: { [id: string]: string }) {

    if (!selectedDates){
      this.selecteDateOverrideIntervals=undefined;
      return 
    }
    //this.selectedDatesFromCalender = selectedDates
    if (this.selecteDateOverrideIntervals) return;

    let timeSlot = this.getTimeInterval(default_startTime_minutes, default_endTime_Minutes)
    this.selecteDateOverrideIntervals = { day: '', isAvailable: true, intervals: [] }
    this.selecteDateOverrideIntervals.intervals.push(timeSlot);

    let selectedDate = "";
    for (var date in selectedDates) {
      selectedDate = date;
      let item = this.availabilityOnDateOverrides.find(e => e.day == date)
      if (item){
        this.selecteDateOverrideIntervals.intervals=item.intervals
      }
      break
    }


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


  private resetTimeIntervals() {
    this.availabilityOnDateOverrides = [];
    this.availabilityInWeek = [];
    day_of_week.forEach(weekDay => {
      let dailyTimeAvailabilities: ITimeIntervalInDay = {
        day: weekDay, isAvailable: true, intervals: []
      }
      this.availabilityInWeek.push(dailyTimeAvailabilities);
    })
  }

  private initMeetingDurationAndTypes() {
    this.meetingDurations.push({ text: "15 min", value: "15" });
    this.meetingDurations.push({ text: "30 min", value: "30" });
    this.meetingDurations.push({ text: "45 min", value: "45" });
    this.meetingDurations.push({ text: "60 min", value: "60" });

  }

  resetMonthlyViewData() {
    let currntDate = new Date(this.selectedYear, this.selectedMonth, 1);
    let firstDayOfMonth = new Date(currntDate.setDate(1));
    let weekDay = new Date(firstDayOfMonth).getDay();
    let beginDateInCalendarMonth = new Date(currntDate.setDate((weekDay - 1) * (-1)));
    let dayNoOfBeginDate = beginDateInCalendarMonth.getDate();
    let dateIncremental = beginDateInCalendarMonth;
    this.availabilityInMonth = [];

    for (let i = 1; i <= 42; i++) {

      let shortMonth = month_of_year[dateIncremental.getMonth()].substring(0, 3);

      let dateName = `${dateIncremental.getFullYear()}-${shortMonth}-${dateIncremental.getDate()}`;

      //let intervalInDay: ITimeIntervalInDay = { day: dateName, intervals: [], isAvailable: true };
      var weekdayName = day_of_week[dateIncremental.getDay()];
      let isOverride = this.availabilityOnDateOverrides.findIndex(e => e.day == dateName) !== -1 ? true : false;
      let intervalsInDay: ITimeInterval[] = [];
      if (isOverride) {
        intervalsInDay = this.availabilityOnDateOverrides.find(e => e.day == dateName)?.intervals!
      }
      else {
        intervalsInDay = this.getWeeklyIntervalsByDate(dateName)!
      }
      let calendarDay: ICalendarDay = {
        isOverride: isOverride,
        dateString: dateName,
        day: dateIncremental.getDate(),
        weekDay: weekdayName,
        intervals: intervalsInDay
      }
      this.availabilityInMonth.push(calendarDay);

      dateIncremental = new Date(dateIncremental.getFullYear(),
        dateIncremental.getMonth(), dateIncremental.getDate() + 1);
    }
  }
  onClickCalendarDay(event: any) {
    let element = event.target.querySelector(".action_buttons");
    element.classList.toggle("is_open");
  }

  onRemoveActionButtonFromCalendarDay(event: any) {
    let element = event.target.querySelector(".action_buttons");
    element.classList.remove("is_open");
  }
  resetView(viewMode: string) {
    this.viewMode = viewMode;
    this.resetMonthlyViewData();
  }
  onClickPreviousMonth(event: any) {
    event.preventDefault();
    if (this.selectedMonth - 1 < 0) {
      this.selectedMonth = 11;
      this.selectedYear--
    }
    else {
      this.selectedMonth--;
    }

    //this.updateTimeIntervalData();
    this.resetMonthlyViewData();
  }

  onClickNextMonth(event: any) {
    event.preventDefault();

    if (this.selectedMonth + 1 > 11) {
      this.selectedMonth = 0;
      this.selectedYear++
    }
    else {
      this.selectedMonth++;
    }
    //this.updateTimeIntervalData();
    this.resetMonthlyViewData();
  }
  isCurrentMonth(): boolean {
    let date = new Date();
    return this.selectedYear == date.getFullYear() && this.selectedMonth == date.getMonth();
  }
  getDay(date: string) {
    return new Date(date).getDate()
  }
  getWeeklyIntervalsByDate(dateString: string): ITimeInterval[] | undefined {
    let date = new Date(dateString);
    let weekDay = day_of_week[date.getDay()];

    let intervalsInDay = this.availabilityInWeek.find(e => e.day == weekDay);
    return intervalsInDay?.intervals;
  }
  getOverrideIntervalsByDate(dateString: string): ITimeInterval[] | undefined {
    let intervalInDate = this.availabilityOnDateOverrides.find(e => e.day == dateString);

    return intervalInDate?.intervals
  }
}

interface ICalendarDay {
  day: number,
  weekDay: string,
  dateString: string,
  isOverride: boolean;
  intervals: ITimeInterval[]
}