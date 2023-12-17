import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { settings_day_of_week, settings_endtime_minutes, settings_starttime_minutes, settings_meeting_day_type_date, settings_meeting_day_type_weekday, settings_month_of_year, settings_time_Slots_List } from '../../utilities/constants-data';
import { TimeZoneData } from '../../interfaces/event-type-interfaces';
import { ListItem } from '../../interfaces/list-item';
import { CalendarComponent } from '../calender/calendar.component';
import { ListOfTimeZone } from '../../utilities/timezone-data';
import { IAvailability, IAvailabilityDetails } from '../../interfaces/availability-interfaces';
import { ITimeIntervalInDay, ITimeInterval, ITimeIntervalsInMonth } from '../../interfaces/ITimeIntervalInDay';
import { DateFunction } from '../../utilities';

@Component({
  selector: 'app-time-availability',
  templateUrl: './time-availability.component.html',
  styleUrls: ['./time-availability.component.scss']
})
export class TimeAvailabilityComponent implements OnInit, AfterViewInit {
  timeSlotsList: string[] = settings_time_Slots_List
  availability: IAvailability | undefined;
  timeZoneList: TimeZoneData[] = ListOfTimeZone;
  filterTimeZoneList: TimeZoneData[] = ListOfTimeZone;
  weekDays = settings_day_of_week;
  monthNames = settings_month_of_year
  meetingDurations: ListItem[] = [];
  availabilityInWeek: ITimeIntervalInDay[] = [];
  availabilityInMonth: ITimeIntervalsInMonth[] = [];
  availabilityOverrides: ITimeIntervalInDay[] = [];
  selecteDateOverride: ITimeIntervalInDay | undefined;
  selectedDatesFromCalender: { [id: string]: string } = {};
  calendarModalEl: Element | null = null;
  dayConfigureModalEl: Element | null = null;
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  selectedDayInWeek: string = "";
  isCurrentMonth: boolean = false;
  selectedTimeZone: any | undefined;
  timeZoneNameFilterText: string = "";
  showCalendarModal: boolean = false;
  showWeekDayConfigureModal: boolean = false;


  @ViewChild(CalendarComponent) calendarComponent!: CalendarComponent;
  @ViewChild('countryContainer') countryContainer: ElementRef | undefined;
  @Input() viewMode: string = "list";
  @Output() TimeAvailabilityChanged = new EventEmitter()

  constructor(
  ) {
  }

  ngAfterViewInit(): void {
  }

  ngOnInit(): void {
    this.calendarModalEl = document.querySelector('.modal-override-dates');
    this.dayConfigureModalEl = document.querySelector(".modal-override-day");
    let currentDate = new Date();
    this.selectedYear = currentDate.getFullYear();
    this.selectedMonth = currentDate.getMonth();

  }

  setAvailability(availability?: IAvailability) {

    this.availability = availability;
    this.selectedTimeZone = this.timeZoneList.find(e => e.name == availability?.timeZone);
    this.prepareWeeklyViewData();
    this.prepareMonthlyViewData();
  }
  prepareWeeklyViewData() {

    this.resetWeeklyTimeIntervals();

    if (!this.availability?.details) return;

    let timeAvailabilityInWeek = this.availability?.details
      .filter(e => e.dayType === settings_meeting_day_type_weekday);


    timeAvailabilityInWeek?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityInWeek.find(e => e.day == item.value);
      dailyTimeAvailabilities!.isAvailable = true;
      dailyTimeAvailabilities!.intervals.push(intervalItem)
      dailyTimeAvailabilities!.isAvailable = dailyTimeAvailabilities!.intervals.length > 0
    })

    let timeOverridesAvailability = this.availability?.details
      .filter(e => e.dayType === settings_meeting_day_type_date);

    timeOverridesAvailability?.forEach(item => {
      let intervalItem = this.getTimeIntervalItem(item.from, item.to);
      let dailyTimeAvailabilities = this.availabilityOverrides.find(e => e.day == item.value);
      if (!dailyTimeAvailabilities) {
        dailyTimeAvailabilities = { day: item.value!, isAvailable: true, intervals: [], isValidData: true };
        this.availabilityOverrides.push(dailyTimeAvailabilities);
      }
      dailyTimeAvailabilities?.intervals.push(intervalItem);
    })
  }

  prepareMonthlyViewData() {
    let currentDate = new Date(this.selectedYear, this.selectedMonth, 1);
    this.isCurrentMonth = DateFunction.isMonthCurrent(currentDate);
    let firstDayOfMonth = new Date(currentDate.setDate(1));
    let weekDay = new Date(firstDayOfMonth).getDay();
    let beginDateInCalendarMonth = new Date(currentDate.setDate((weekDay - 1) * (-1)));
    let dateIncremental = beginDateInCalendarMonth;
    this.availabilityInMonth = [];

    for (let i = 1; i <= 42; i++) {

      let shortMonth = settings_month_of_year[dateIncremental.getMonth()].substring(0, 3);

      let dateName = `${dateIncremental.getFullYear()}-${shortMonth}-${dateIncremental.getDate()}`;

      var weekdayName = settings_day_of_week[dateIncremental.getDay()];
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
        let item: ITimeInterval = {
          startTime: e.startTime,
          startTimeInMinute: e.startTimeInMinute,
          endTime: e.endTime,
          endTimeInMinute: e.endTimeInMinute,
          errorMessage: e.errorMessage
        }
        intervalsClone.push(item)
      });
      let isPastDate = DateFunction.isDayPast(dateIncremental);
      let calendarDay: ITimeIntervalsInMonth = {
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
    let startTime_Minutes = settings_starttime_minutes;
    let endTime_Minutes = settings_endtime_minutes;

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

    this.validateTimeIntervalsOfDay(timeInteralsInDay);

  }

  onRemoveTimeInterval(index: number, dailyTimeAvailabilities: ITimeIntervalInDay) {

    dailyTimeAvailabilities.intervals.splice(index, 1);

    if (dailyTimeAvailabilities.intervals.length == 0)
      dailyTimeAvailabilities.isAvailable = false;

    this.validateTimeIntervalsOfDay(dailyTimeAvailabilities);
  }

  onLostFocus(e: Event, index: number, isEndTime: boolean, dailyTimeAvailabilities: ITimeIntervalInDay) {

    dailyTimeAvailabilities.isValidData = true;

    let htmlElement = e.target as HTMLInputElement;
    let timeValue = htmlElement.value;
    let intervalItem = dailyTimeAvailabilities.intervals[index];
    if (timeValue === undefined || timeValue.trim() === '') {
      intervalItem.errorMessage = "Invalid time";
      dailyTimeAvailabilities.isValidData = false;
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
      if (totalMinutes == intervalItem.endTimeInMinute) return;
      intervalItem.endTimeInMinute = totalMinutes
      intervalItem.endTime = time;
    }
    else {
      if (totalMinutes == intervalItem.startTimeInMinute) return;
      intervalItem.startTimeInMinute = totalMinutes
      intervalItem.startTime = time;
    }

    if (intervalItem.endTimeInMinute < intervalItem.startTimeInMinute) {
      intervalItem.errorMessage = "End time should be after than start time."
      dailyTimeAvailabilities.isValidData = false;
      return;
      //htmlElement.focus();
    }
    else {
      intervalItem.errorMessage = "";
    }

    if (dailyTimeAvailabilities.isValidData)
      this.validateTimeIntervalsOfDay(dailyTimeAvailabilities)
  }

  onOpenCalendarModal(modalName: string, defaultDate?: Date) {

    this.calendarComponent.resetCalendar(defaultDate);
    this.showCalendarModal = true;

  }
  onCloseCalendarModal() {
    this.selecteDateOverride = undefined;
    this.selectedDatesFromCalender = {};
    this.showCalendarModal = false;
  }

  onApplyCalendarDateChanges() {

    for (let date in this.selectedDatesFromCalender) {

      let index = this.availabilityOverrides.findIndex(e => e.day == date);

      if (index > -1) this.availabilityOverrides.splice(index, 1);

      if (this.selecteDateOverride?.intervals.length! > 0) {
        let timeIntervalInDay: ITimeIntervalInDay = { day: date, isAvailable: true, intervals: [], isValidData: true };

        this.selecteDateOverride?.intervals.forEach(interval => {
          timeIntervalInDay.intervals.push(interval);
        })
        this.availabilityOverrides.push(timeIntervalInDay);
      }
    }

    this.availabilityOverrides.sort((a, b) => new Date(a.day).getTime() - new Date(b.day).getTime());

    this.onCloseCalendarModal();
    this.prepareMonthlyViewData();
    this.validateTimeIntervalsOfAllDays();
  }

  onCloseWeekdayConfigureModal() {
    this.showWeekDayConfigureModal = false;
  }

  onApplyWeekDayChanges() {

    this.showWeekDayConfigureModal = false;
    let date = new Date(this.selecteDateOverride?.day!);
    let weekDayName = settings_day_of_week[date.getDay()];
    let weekDayInvevals = this.availabilityInWeek.find(e => e.day == weekDayName);
    let intervals = this.selecteDateOverride?.intervals.slice()!
    weekDayInvevals!.intervals = intervals;
    this.prepareMonthlyViewData();
    this.validateTimeIntervalsOfAllDays();
  }
  onRemoveOverrideData(index: number) {
    this.availabilityOverrides.splice(index, 1);
    this.validateTimeIntervalsOfAllDays();
  }

  onEditAvailabilityForOverrideDate(timeIntervalInDay: ITimeIntervalInDay) {

    const jsonString = JSON.stringify(timeIntervalInDay);

    let cloneCopy = JSON.parse(jsonString) as ITimeIntervalInDay;

    this.selecteDateOverride = {
      day: timeIntervalInDay.day, isAvailable: true, intervals: cloneCopy.intervals, isValidData: true
    };

    let selectedDate = new Date(this.selecteDateOverride.day);
    this.onOpenCalendarModal('modal-override-dates', selectedDate);
  }

  onEditAvailabilityForDate(calendarDay: ITimeIntervalsInMonth) {

    let cloneIntervals = calendarDay.intervals.slice();

    this.selecteDateOverride = {
      day: calendarDay.dateString, isAvailable: true, intervals: cloneIntervals, isValidData: true
    };
    let selectedDate = new Date(this.selecteDateOverride.day);
    this.onOpenCalendarModal('modal-override-dates', selectedDate);
  }

  onEditAvailabilityForWeekDay(calendarDay: ITimeIntervalsInMonth) {

    this.selecteDateOverride = {
      day: calendarDay.dateString, isAvailable: true, intervals: calendarDay.intervals.slice(), isValidData: true
    };

    this.selectedDayInWeek = calendarDay.weekDay;
    this.showWeekDayConfigureModal = true;
  }

  onHandledCalendarClicked(selectedDates: { [id: string]: string }) {

    let filtered = Object.fromEntries(Object.entries(selectedDates)
      .filter(([k, v]) => v != undefined));

    if (Object.keys(filtered).length == 0) {
      this.selecteDateOverride?.intervals.splice(0, 100);
    }
    this.selectedDatesFromCalender = selectedDates
    if (this.selecteDateOverride) return;

    let timeSlot = this.getTimeIntervalItem(settings_starttime_minutes, settings_endtime_minutes)
    this.selecteDateOverride = { day: '', isAvailable: true, intervals: [], isValidData: true }
    this.selecteDateOverride.intervals.push(timeSlot);

    for (var date in selectedDates) {
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
        dailyTimeAvailabilities.intervals.push(this.getTimeIntervalItem(settings_starttime_minutes, settings_endtime_minutes));
    }

    this.validateTimeIntervalsOfDay(dailyTimeAvailabilities);
  }

  getAvailability(): IAvailability | undefined {
    this.availability?.details.splice(0, 100);
    this.availability!.timeZone = this.selectedTimeZone?.name!;
    this.availabilityInWeek.filter(e => e.isAvailable).forEach(weekday => {
      weekday.intervals.forEach(intervalITem => {
        let item: IAvailabilityDetails = {
          dayType: settings_meeting_day_type_weekday,
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
          dayType: settings_meeting_day_type_date,
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

  onClickDayInMonthView(event: any) {
    let element = event.target.querySelector(".action_buttons");
    if (!element) {
      element = event.target.parentElement.parentElement.querySelector(".action_buttons")
    }
    if (element)
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
    let weekDay = settings_day_of_week[date.getDay()];

    let intervalsInDay = Object.assign({}, this.availabilityInWeek.find(e => e.day == weekDay));
    return intervalsInDay?.intervals;
  }

  getOverrideIntervalsByDate(dateString: string): ITimeInterval[] | undefined {
    let intervalInDate = this.availabilityOverrides.find(e => e.day == dateString);

    return intervalInDate?.intervals
  }

  onResetToWeeklyHours(calendarDay: ITimeIntervalsInMonth) {
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

  onToggleCountryDropdownBox() {
    this.countryContainer?.nativeElement.classList.toggle('active');
  }

  onFilterTimeZoneChanged(event: any) {
    if (this.timeZoneNameFilterText.trim() !== '')
      this.filterTimeZoneList = this.timeZoneList
        .filter(e => e.name.toLowerCase()
          .indexOf(this.timeZoneNameFilterText.toLowerCase()) > -1)
    else
      this.filterTimeZoneList = this.timeZoneList;

  }
  onSelectTimeZone(timeZoneItem: TimeZoneData) {
    this.selectedTimeZone = timeZoneItem;
    this.timeZoneNameFilterText = "";
    this.onToggleCountryDropdownBox();
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
    settings_day_of_week.forEach(weekDay => {
      let dailyTimeAvailabilities: ITimeIntervalInDay = {
        day: weekDay, isAvailable: false, intervals: [], isValidData: true
      }
      this.availabilityInWeek.push(dailyTimeAvailabilities);
    })
  }

  private validateTimeIntervalsOfDay(timeIntervalsInDay: ITimeIntervalInDay) {

    //timeIntervalsInDay.intervals.forEach(e => e.errorMessage = "");

    let intervals = timeIntervalsInDay.intervals;

    //if (intervals.length == 0) return;

    timeIntervalsInDay.isValidData = true;

    // if (intervals.length == 1) {
    //   let item = intervals[0];
    //   if (item.startTimeInMinute >= item.endTimeInMinute) {
    //     item.errorMessage = "End time should be after than start time."
    //   }
    //   return;
    // }

    for (let i = 0; i < intervals.length; i++) {
      let item = intervals[i];
      if (item.startTimeInMinute >= item.endTimeInMinute) {
        item.errorMessage = "End time should be after than start time."
        timeIntervalsInDay.isValidData = false;
        break
      }
      for (let j = i + 1; j < intervals.length; j++) {
        let item2 = intervals[j];
        if (
          (item.startTimeInMinute >= item2.startTimeInMinute && item.startTimeInMinute <= item2.endTimeInMinute) ||
          (item.endTimeInMinute >= item2.startTimeInMinute && item.endTimeInMinute <= item2.endTimeInMinute)
        ) {
          item.errorMessage = "Times overlaping with other intervals";
          item2.errorMessage = "Times overlaping with other intervals";
          timeIntervalsInDay.isValidData = false;
          break;
        }
      }
    }

    this.validateTimeIntervalsOfAllDays();
  }

  private validateTimeIntervalsOfAllDays() {
    const invalidWeekDaysIntervals = this.availabilityInWeek.filter(e => e.isAvailable && e.intervals.length > 0 && !e.isValidData);
    const invalidOverrideDaysIntervals = this.availabilityOverrides.filter(e => e.intervals.length > 0 && !e.isValidData);
    const isValid = invalidWeekDaysIntervals.length == 0 && invalidOverrideDaysIntervals.length == 0;

    const isValidToNotifyChange = !this.showCalendarModal && !this.showWeekDayConfigureModal

    if (isValid && isValidToNotifyChange) {
      this.TimeAvailabilityChanged.emit(true);
    }
    else {
      this.TimeAvailabilityChanged.emit(false);
    }
  }
}

