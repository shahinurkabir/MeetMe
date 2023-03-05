import { AfterViewInit, Component, Input, OnInit, Output, ViewChild } from '@angular/core';
import { day_of_week, default_endTime_Minutes, default_startTime_minutes, meeting_day_type_date, meeting_day_type_weekday, month_of_year } from '../../constants/default-data';
import { TimeZoneData } from '../../models/eventtype';
import { IAvailability } from '../../models/IAvailability';
import { IAvailabilityDetails } from '../../models/IAvailabilityDetails';
import { ITimeInterval } from '../../models/ITimeInterval';
import { ITimeIntervalInDay } from '../../models/ITimeIntervalInDay';
import { ListItem } from '../../models/list-item';
import { TimeZoneService } from '../../services/timezone.service';
import { toggleModalDialog } from '../../utilities/functions';
import { date } from '../../utils/functions/date-functions';
import { CalendarComponent } from '../calender/calendar.component';
import { ModalService } from '../modal/modalService';

@Component({
  selector: 'app-time-availability',
  templateUrl: './time-availability.component.html',
  styleUrls: ['./time-availability.component.scss']
})
export class TimeAvailabilityComponent implements OnInit, AfterViewInit {

  availability: IAvailability | undefined;
  timeZoneList: TimeZoneData[] = [];
  weekDays = day_of_week;
  monthNames = month_of_year
  meetingDurations: ListItem[] = [];
  //availabilityList: EventAvailabilityDetailItem[] = [];
  availabilityInWeek: ITimeIntervalInDay[] = [];
  availabilityInMonth: ICalendarDay[] = [];
  availabilityOverrides: ITimeIntervalInDay[] = [];
  selecteDateOverride: ITimeIntervalInDay | undefined;
  selectedDatesFromCalender: { [id: string]: string } = {};
  viewMode: string = "list";
  calendarModalEl: Element | null = null;
  dayConfigureModalEl: Element | null = null;
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  selectedDayInWeek: string = "";
  isCurrentMonth: boolean = false;
  selectedTimeZoneId: number = -1;
  @ViewChild(CalendarComponent) calendarComponent!: CalendarComponent;

  constructor(
    private timeZoneService: TimeZoneService,
    private modalService:ModalService
    ) {
    this.loadTimeZoneList();
  }
  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    this.calendarModalEl = document.querySelector('.modal-override-dates');
    this.dayConfigureModalEl = document.querySelector(".modal-override-day");
    let currentDate = new Date();
    this.selectedYear = currentDate.getFullYear();
    this.selectedMonth = currentDate.getMonth();
    // this.prepareWeeklyViewData();
    // this.prepareMonthlyViewData();

  }
  loadTimeZoneList() {
    this.timeZoneService.getList().subscribe(res => {
      this.timeZoneList = res;
    })
  }
  setAvailability(availability: IAvailability) {
    this.availability = availability;
    this.selectedTimeZoneId = availability.timeZoneId;
  }
  prepareWeeklyViewData() {

    this.resetWeeklyTimeIntervals();

    let timeAvailabilityInWeek = this.availability?.details
      .filter(e => e.dayType === meeting_day_type_weekday);


    timeAvailabilityInWeek?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityInWeek.find(e => e.day == item.value);
      dailyTimeAvailabilities!.isAvailable = true;
      dailyTimeAvailabilities!.intervals.push(intervalItem)
      dailyTimeAvailabilities!.isAvailable = dailyTimeAvailabilities!.intervals.length > 0
    })

    let timeOverridesAvailability = this.availability?.details
      .filter(e => e.dayType === meeting_day_type_date);

    timeOverridesAvailability?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityOverrides.find(e => e.day == item.value);
      if (!dailyTimeAvailabilities) {
        dailyTimeAvailabilities = { day: item.value!, isAvailable: true, intervals: [] };
        this.availabilityOverrides.push(dailyTimeAvailabilities);
      }
      dailyTimeAvailabilities?.intervals.push(intervalItem);
    })
  }

  prepareMonthlyViewData() {
    let currentDate = new Date(this.selectedYear, this.selectedMonth, 1);
    this.isCurrentMonth = date.isMonthCurrent(currentDate);
    let firstDayOfMonth = new Date(currentDate.setDate(1));
    let weekDay = new Date(firstDayOfMonth).getDay();
    let beginDateInCalendarMonth = new Date(currentDate.setDate((weekDay - 1) * (-1)));
    let dateIncremental = beginDateInCalendarMonth;
    this.availabilityInMonth = [];

    for (let i = 1; i <= 42; i++) {

      let shortMonth = month_of_year[dateIncremental.getMonth()].substring(0, 3);

      let dateName = `${dateIncremental.getFullYear()}-${shortMonth}-${dateIncremental.getDate()}`;

      var weekdayName = day_of_week[dateIncremental.getDay()];
      let isOverride = this.availabilityOverrides.findIndex(e => e.day == dateName) !== -1 ? true : false;
      let intervalsInDay: ITimeInterval[] = [];

      if (isOverride) {
        intervalsInDay = this.availabilityOverrides.find(e => e.day == dateName)?.intervals!
      }
      else {
        intervalsInDay = this.getTimeIntervalsByDay(dateName)!

      }
      let intervalsClone: ITimeInterval[] = [];
      intervalsInDay.forEach(e => {
        let item: ITimeInterval = { startTime: e.startTime, startTimeInMinute: e.startTimeInMinute, endTime: e.endTime, endTimeInMinute: e.endTimeInMinute, errorMessage: e.errorMessage }
        intervalsClone.push(item)
      });
      let isPastDate = date.isDayPast(dateIncremental);
      let calendarDay: ICalendarDay = {
        isOverride: isOverride,
        dateString: dateName,
        day: dateIncremental.getDate(),
        weekDay: weekdayName,
        intervals: intervalsClone,
        isPastDate: isPastDate
      }
      this.availabilityInMonth.push(calendarDay);

      dateIncremental = new Date(dateIncremental.getFullYear(),
        dateIncremental.getMonth(), dateIncremental.getDate() + 1);
    }

    console.log(this.availabilityInMonth);

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

  onOpenCalendarModal(modalName:string,defaultDate?: Date) {
    this.calendarComponent.resetSelection(defaultDate);
    this.modalService.open(modalName)
    //toggleModalDialog(this.calendarModalEl);

  }
  onCloseCalendarModal() {
    this.selecteDateOverride = undefined;
    this.selectedDatesFromCalender = {};
    this.modalService.close()
    //toggleModalDialog(this.calendarModalEl);
  }
  onApplyCalendarDateChanges() {

    for (let date in this.selectedDatesFromCalender) {

      let index = this.availabilityOverrides.findIndex(e => e.day == date);

      if (index > -1) this.availabilityOverrides.splice(index, 1);

      if (this.selecteDateOverride?.intervals.length! > 0) {
        let timeIntervalInDay: ITimeIntervalInDay = { day: date, isAvailable: true, intervals: [] };

        this.selecteDateOverride?.intervals.forEach(interval => {
          timeIntervalInDay.intervals.push(interval);
        })
        this.availabilityOverrides.push(timeIntervalInDay);
      }
    }

    this.availabilityOverrides.sort((a, b) => new Date(a.day).getTime() - new Date(b.day).getTime());

    this.onCloseCalendarModal();
    this.prepareMonthlyViewData();
  }

  onRemoveOverrideData(index: number) {
    this.availabilityOverrides.splice(index, 1);
  }

  onEditAvailabilityForOverrideDate(index: number) {
    this.selecteDateOverride = this.availabilityOverrides[index];
    let selectedDate = new Date(this.selecteDateOverride.day);
    //this.calendarComponent.resetSelection(selectedDate);
    this.onOpenCalendarModal('modal-override-dates',selectedDate);
  }
  onEditAvailabilityForDate(calendarDay: ICalendarDay) {

    let cloneIntervals = calendarDay.intervals.slice();

    this.selecteDateOverride = {
      day: calendarDay.dateString, isAvailable: true, intervals: cloneIntervals
    };
    let selectedDate = new Date(this.selecteDateOverride.day);
    //this.calendarComponent.resetSelection(selectedDate);
    this.onOpenCalendarModal('modal-override-dates',selectedDate);
  }

  onEditAvailabilityForWeekDay(calendarDay: ICalendarDay) {

    this.selecteDateOverride = {
      day: calendarDay.dateString, isAvailable: true, intervals: calendarDay.intervals.slice()
    };

    this.selectedDayInWeek = calendarDay.weekDay;

    //toggleModalDialog(this.dayConfigureModalEl);
    this.modalService.open('modal-override-day')
  }

  onHandledCalendarClicked(selectedDates: { [id: string]: string }) {

    let filtered = Object.fromEntries(Object.entries(selectedDates)
      .filter(([k, v]) => v != undefined));

    if (Object.keys(filtered).length == 0) {
      this.selecteDateOverride?.intervals.splice(0, 100);
    }
    this.selectedDatesFromCalender = selectedDates
    if (this.selecteDateOverride) return;

    let timeSlot = this.getTimeIntervalItem(default_startTime_minutes, default_endTime_Minutes)
    this.selecteDateOverride = { day: '', isAvailable: true, intervals: [] }
    this.selecteDateOverride.intervals.push(timeSlot);

    let selectedDate = "";
    for (var date in selectedDates) {
      selectedDate = date;
      let item = this.availabilityOverrides.find(e => e.day == date)
      if (item) {
        this.selecteDateOverride.intervals = item.intervals
      }
      break
    }


  }
  onWeekdaySelection(dailyTimeAvailabilities: ITimeIntervalInDay) {
    dailyTimeAvailabilities.isAvailable = !dailyTimeAvailabilities.isAvailable;
    if (dailyTimeAvailabilities.isAvailable) {
      if (dailyTimeAvailabilities.intervals.length == 0)
        dailyTimeAvailabilities.intervals.push(this.getTimeIntervalItem(default_startTime_minutes, default_endTime_Minutes));
    }
  }

  getAvailability(): IAvailability | undefined {
    this.availability?.details.splice(0, 100);
    this.availability?.timeZoneId != this.selectedTimeZoneId;
    this.availabilityInWeek.filter(e => e.isAvailable).forEach(weekday => {
      weekday.intervals.forEach(intervalITem => {
        let item: IAvailabilityDetails = {
          dayType: meeting_day_type_weekday,
          value: weekday.day,
          from: intervalITem.startTimeInMinute,
          to: intervalITem.endTimeInMinute,
          stepId: 0
        };
        this.availability?.details.push(item);
      });
    });
    this.availabilityOverrides.forEach(overrrideItem => {
      overrrideItem.intervals.forEach(intervalITem => {
        let item: IAvailabilityDetails = {
          dayType: meeting_day_type_date,
          value: overrrideItem.day,
          from: intervalITem.startTimeInMinute,
          to: intervalITem.endTimeInMinute,
          stepId: 0
        };
        this.availability?.details.push(item);
      });
    });

    return this.availability;
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
    this.availabilityOverrides = [];
    this.availabilityInWeek = [];
    day_of_week.forEach(weekDay => {
      let dailyTimeAvailabilities: ITimeIntervalInDay = {
        day: weekDay, isAvailable: false, intervals: []
      }
      this.availabilityInWeek.push(dailyTimeAvailabilities);
    })
  }


  onClickDayInMonthView(event: any) {
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
    let intervalInDate = this.availabilityOverrides.find(e => e.day == dateString);

    return intervalInDate?.intervals
  }

  onResetToWeeklyHours(calendarDay: ICalendarDay) {
    let timeIntervalInDay = this.getTimeIntervalsByDay(calendarDay.dateString);
    calendarDay.isOverride = false;
    calendarDay.intervals = timeIntervalInDay!;
    this.removeDayFromOverrrideList(calendarDay.dateString);
  }
  removeDayFromOverrrideList(dateString: string) {
    let dateIndex = this.availabilityOverrides.findIndex(e => e.day == dateString);
    if (dateIndex != -1) {
      this.availabilityOverrides.splice(dateIndex, 1);
    }
  }
  onCloseWeekdayConfigureModal() {
    //toggleModalDialog(this.dayConfigureModalEl)
    this.modalService.close();
  }
  onApplyWeekDayChanges() {
    let date = new Date(this.selecteDateOverride?.day!);
    let weekDayName = day_of_week[date.getDay()];
    let weekDayInvevals = this.availabilityInWeek.find(e => e.day == weekDayName);
    let intervals = this.selecteDateOverride?.intervals.slice()!
    weekDayInvevals!.intervals = intervals;

    //toggleModalDialog(this.dayConfigureModalEl)
    this.modalService.close();
    this.prepareMonthlyViewData();
  }

}

interface ICalendarDay {
  day: number,
  weekDay: string,
  dateString: string,
  isOverride: boolean;
  intervals: ITimeInterval[],
  isPastDate: boolean
}