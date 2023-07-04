import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarComponent, TimezoneControlComponent, IEventTimeAvailability, TimeZoneData, EventTypeService, convertTimeZoneLocalTime } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-event-type-calendar',
  templateUrl: './eventtype-calendar.component.html',
  styleUrls: ['./eventtype-calendar.component.scss']
})
export class EventTypeCalendarComponent implements OnInit, OnDestroy {
  @ViewChild(CalendarComponent, { static: true }) calendarComponent!: CalendarComponent;
  @ViewChild("timezoneControl", { static: true }) timezoneControl: TimezoneControlComponent | undefined;
  destroyed$: Subject<boolean> = new Subject<boolean>();
  availableTimeSlots: IEventTimeAvailability[] = [];
  day: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  selectedYear: number = 0;
  selectedMonth: number = 0;
  is24HourFormat: boolean = false;
  eventTypeId: string = "";
  timeZoneName: string = "";
  selectedDate: string | null = null;
  selectedYearMonth: string | null = null;
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.timeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;

  }

  ngOnInit(): void {
    this.eventTypeId = this.route.snapshot.queryParamMap.get("id") ?? "";
    this.selectedDate = this.route.snapshot.queryParamMap.get('date');
    this.selectedYearMonth = this.route.snapshot.queryParamMap.get("month");

    if (this.selectedYearMonth) {
      let yearMonth = this.selectedYearMonth.split("-");
      this.selectedYear = parseInt(yearMonth[0]);
      this.selectedMonth = parseInt(yearMonth[1]) - 1;
      this.calendarComponent.moveTo(this.selectedYear, this.selectedMonth);
    }
    else {
      this.calendarComponent.resetCalendar();
    }
  }

  ngViewAfterInit() {
  }

  onHandledCalendarClicked(e: any) {
    console.log(`day changed ${e}`);

    if (Object.keys(e).length == 0) return;

    this.selectedDate = Object.keys(e)[Object.keys(e).length - 1];
    this.showAvailableTimeSlotsInDay();

    this.addParamDate();
  }

  onHandledMonthChange(e: any) {

    console.log(`month changed ${e}`);
    this.selectedYear = e.year;
    this.selectedMonth = e.month;
    this.addParamMonth();
    this.loadEventCalendarData();
  }

  loadEventCalendarData() {

    let daysInMonth = new Date(this.selectedYear, this.selectedMonth + 1, 0).getDate();

    let fromDate = this.selectedYear + "-" + (this.selectedMonth + 1) + "-01";
    let toDate = this.selectedYear + "-" + (this.selectedMonth + 1) + "-" + daysInMonth;

    let currentDate = new Date(new Date().toLocaleString("en-US", { timeZone: this.timeZoneName }));
    let isCurrentMonth = currentDate.getMonth() == this.selectedMonth && currentDate.getFullYear() == this.selectedYear;
    if (isCurrentMonth) {
      fromDate = this.selectedYear + "-" + (this.selectedMonth + 1) + "-" + currentDate.getDate();
    }

    this.eventTypeService.getCalendarAvailability(this.eventTypeId, this.timeZoneName, fromDate, toDate)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.availableTimeSlots = response;
          this.updateTimeLocalTime();
          this.disableNotAvailableDays(fromDate, toDate, response);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
  }
  disableNotAvailableDays(fromDate: string, toDate: string, avalableTimeSlots: IEventTimeAvailability[]) {

    let from = new Date(fromDate);
    let to = new Date(toDate);
    let days: number[] = [];
    for (let index = from.getDate(); index <= to.getDate(); index++) {
      days.push(index);
    }
    let daysHasData = avalableTimeSlots.map(e => new Date(e.date).getDate());
    daysHasData.forEach((day) => {
      let index = days.indexOf(day);
      if (index != -1) {
        days.splice(index, 1);
      }
    });
    if (days.length == 0) return;
    this.calendarComponent.disableDays(days);
  }
  showAvailableTimeSlotsInDay() {
    this.day = this.availableTimeSlots.find(e => e.date == this.selectedDate);
  }
  onLoadedTimezoneData(e: any) {
    console.log(e);
  }
  onChangedTimezone(e: TimeZoneData) {
    console.log(e);
    this.selectedTimeZone = e;
    this.timeZoneName = e.name;
    this.calendarComponent.resetTimeZone(this.timeZoneName);
  }

  onChangedHourFormat(is24HourFormat: boolean) {
    this.is24HourFormat = is24HourFormat;
    this.updateTimeLocalTime();
  }

  updateTimeLocalTime() {
    this.availableTimeSlots.forEach((day) => {
      day.slots.forEach((slot) => {
        slot.startAtTimeOnly = convertTimeZoneLocalTime(new Date(slot.startAt), this.is24HourFormat, this.selectedTimeZone?.name!);
      });
    });
  }
  addParamMonth() {
    // changes the route without moving from the current view or
    // triggering a navigation event,
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        month: `${this.selectedYear}-${this.selectedMonth + 1}`,
      },
      queryParamsHandling: 'merge',
      // preserve the existing query params in the route
      skipLocationChange: false
      // do not trigger navigation
    });
  }
  addParamDate() {
    // changes the route without moving from the current view or
    // triggering a navigation event,
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        date: this.selectedDate
      },
      queryParamsHandling: 'merge',
      // preserve the existing query params in the route
      skipLocationChange: false
      // do not trigger navigation
    });
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
