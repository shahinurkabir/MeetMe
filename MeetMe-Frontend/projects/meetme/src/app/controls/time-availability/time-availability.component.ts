import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { interval, observable } from 'rxjs';
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
export class TimeAvailabilityComponent implements OnInit, AfterViewInit {
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
  dayConfigureModalEl: Element | null = null;
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  selectedDayForConfigure: string = "";
  @ViewChild(CalendarComponent) calendarComponent!: CalendarComponent;
  constructor() {

  }
  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    this.calendarModalEl = document.querySelector('.modal-override-dates');
    this.dayConfigureModalEl = document.querySelector(".modal-override-day");
    let currentDate = new Date();
    this.selectedYear = currentDate.getFullYear();
    this.selectedMonth = currentDate.getMonth();
    this.prepareWeeklyViewData();
    this.prepareMonthlyViewData();
  }

  prepareWeeklyViewData() {

    this.resetWeeklyTimeIntervals();

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

  prepareMonthlyViewData() {
    let currntDate = new Date(this.selectedYear, this.selectedMonth, 1);
    let firstDayOfMonth = new Date(currntDate.setDate(1));
    let weekDay = new Date(firstDayOfMonth).getDay();
    let beginDateInCalendarMonth = new Date(currntDate.setDate((weekDay - 1) * (-1)));
    let dateIncremental = beginDateInCalendarMonth;
    this.availabilityInMonth = [];

    for (let i = 1; i <= 42; i++) {

      let shortMonth = month_of_year[dateIncremental.getMonth()].substring(0, 3);

      let dateName = `${dateIncremental.getFullYear()}-${shortMonth}-${dateIncremental.getDate()}`;

      var weekdayName = day_of_week[dateIncremental.getDay()];
      let isOverride = this.availabilityOnDateOverrides.findIndex(e => e.day == dateName) !== -1 ? true : false;
      let intervalsInDay: ITimeInterval[] = [];

      if (isOverride) {
        intervalsInDay = this.availabilityOnDateOverrides.find(e => e.day == dateName)?.intervals!
      }
      else {
        intervalsInDay = this.getTimeIntervalsByDay(dateName)!

      }
      let intervalsClone: ITimeInterval[] = [];
      intervalsInDay.forEach(e => {
        let item: ITimeInterval = { startTime: e.startTime, startTimeInMinute: e.startTimeInMinute, endTime: e.endTime, endTimeInMinute: e.endTimeInMinute, errorMessage: e.errorMessage }
        intervalsClone.push(item)
      });
      let calendarDay: ICalendarDay = {
        isOverride: isOverride,
        dateString: dateName,
        day: dateIncremental.getDate(),
        weekDay: weekdayName,
        intervals: intervalsClone
      }
      this.availabilityInMonth.push(calendarDay);

      dateIncremental = new Date(dateIncremental.getFullYear(),
        dateIncremental.getMonth(), dateIncremental.getDate() + 1);
    }
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

    let timeSlot = this.getTimeIntervalItem(startTime_Minutes, endTime_Minutes);

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

  onOpenCalendarModal(dateSelect?: number) {
    //event.preventDefault();   
    //this.selectedDatesFromCalender={};
    this.calendarComponent.resetSelection(dateSelect);
    toggleModalDialog(this.calendarModalEl);
  }

  onApplyCalendarDateChanges() {

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

    this.onCloseCalendarModal();
    this.prepareMonthlyViewData();
  }

  onCloseCalendarModal() {
    this.selecteDateOverrideIntervals = undefined;
    this.selectedDatesFromCalender = {};
    toggleModalDialog(this.calendarModalEl);
  }

  onRemoveOverrideData(index: number) {
    this.availabilityOnDateOverrides.splice(index, 1);
  }

  onEditOverrideData(index: number) {
    this.selecteDateOverrideIntervals = this.availabilityOnDateOverrides[index];
    let daySelect = new Date(this.selecteDateOverrideIntervals.day).getDate();
    this.calendarComponent.resetSelection(daySelect);
    this.onOpenCalendarModal(daySelect);
  }
  onEditIntervalsInDate(calendarDay: ICalendarDay) {
    let cloneIntervals = calendarDay.intervals.slice()
    this.selecteDateOverrideIntervals = {
      day: calendarDay.dateString, isAvailable: true, intervals: cloneIntervals
    };

    let daySelect = new Date(calendarDay.dateString).getDate();
    this.calendarComponent.resetSelection(daySelect);
    this.onOpenCalendarModal(daySelect);
  }

  onEditIntervalsInDay(calendarDay: ICalendarDay) {
    this.selecteDateOverrideIntervals = {
      day: calendarDay.dateString, isAvailable: true, intervals: calendarDay.intervals.slice()
    };
    this.selectedDayForConfigure = calendarDay.weekDay;

    toggleModalDialog(this.dayConfigureModalEl);
  }

  onDateClicked(selectedDates: { [id: string]: string }) {

    let filtered = Object.fromEntries(Object.entries(selectedDates)
      .filter(([k, v]) => v != undefined));

    if (Object.keys(filtered).length == 0) {
      this.selecteDateOverrideIntervals?.intervals.splice(0, 100);
    }
    this.selectedDatesFromCalender = selectedDates
    if (this.selecteDateOverrideIntervals) return;

    let timeSlot = this.getTimeIntervalItem(default_startTime_minutes, default_endTime_Minutes)
    this.selecteDateOverrideIntervals = { day: '', isAvailable: true, intervals: [] }
    this.selecteDateOverrideIntervals.intervals.push(timeSlot);

    let selectedDate = "";
    for (var date in selectedDates) {
      selectedDate = date;
      let item = this.availabilityOnDateOverrides.find(e => e.day == date)
      if (item) {
        this.selecteDateOverrideIntervals.intervals = item.intervals
      }
      break
    }


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


  private resetWeeklyTimeIntervals() {
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



  onClickCalendarDay(event: any) {
    let element = event.target.querySelector(".action_buttons");
    element.classList.toggle("is_open");
  }

  onRemoveActionButtonFromCalendarDay(event: any) {
    let element = event.target.querySelector(".action_buttons");
    element.classList.remove("is_open");
  }
  onToggleView(viewMode: string) {
    this.viewMode = viewMode;
    this.prepareMonthlyViewData();
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

    this.prepareMonthlyViewData();
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
    this.prepareMonthlyViewData();
  }

  isCurrentMonth(): boolean {
    let date = new Date();
    return this.selectedYear == date.getFullYear() && this.selectedMonth == date.getMonth();
  }
  getDay(date: string) {
    return new Date(date).getDate()
  }
  getTimeIntervalsByDay(dateString: string): ITimeInterval[] | undefined {
    let date = new Date(dateString);
    let weekDay = day_of_week[date.getDay()];

    let intervalsInDay = Object.assign({}, this.availabilityInWeek.find(e => e.day == weekDay));
    return intervalsInDay?.intervals;
  }
  getOverrideIntervalsByDate(dateString: string): ITimeInterval[] | undefined {
    let intervalInDate = this.availabilityOnDateOverrides.find(e => e.day == dateString);

    return intervalInDate?.intervals
  }

  onResetToWeeklyHours(calendarDay: ICalendarDay) {
    let timeIntervalInDay = this.getTimeIntervalsByDay(calendarDay.dateString);
    calendarDay.isOverride = false;
    calendarDay.intervals = timeIntervalInDay!;
    this.removeDayFromOverrrideList(calendarDay.dateString);
  }
  removeDayFromOverrrideList(dateString: string) {
    let dateIndex = this.availabilityOnDateOverrides.findIndex(e => e.day == dateString);
    if (dateIndex != -1) {
      this.availabilityOnDateOverrides.splice(dateIndex, 1);
    }
  }
  onCloseWeekdayConfigureModal() {
    toggleModalDialog(this.dayConfigureModalEl)
  }
  onApplyWeekDayChanges() {
    let date = new Date(this.selecteDateOverrideIntervals?.day!);
    let weekDayName = day_of_week[date.getDay()];
    let weekDayInvevals=this.availabilityInWeek.find(e=>e.day==weekDayName);
    let intervals=this.selecteDateOverrideIntervals?.intervals.slice()!
    weekDayInvevals!.intervals=intervals;

    toggleModalDialog(this.dayConfigureModalEl)

    this.prepareMonthlyViewData();
  }
}

interface ICalendarDay {
  day: number,
  weekDay: string,
  dateString: string,
  isOverride: boolean;
  intervals: ITimeInterval[]
}