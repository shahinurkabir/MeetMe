import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CalendarComponent } from '../../controls/calender/calendar.component';
import { BookingService } from '../../services/booking.service';
import { IEventTimeAvailability } from '../../models/calendar';
import { TimeZoneData } from '../../models/eventtype';
import { TimezoneControlComponent } from '../../controls/timezone-control/timezone-control.component';
import { convertTimeZoneLocalTime } from '../../utilities/time.extensions';
import { ActivatedRoute, Router } from '@angular/router';
import { concat } from 'rxjs';

@Component({
  selector: 'app-event-type-calendar',
  templateUrl: './eventtype-calendar.component.html',
  styleUrls: ['./eventtype-calendar.component.scss']
})
export class EventTypeCalendarComponent implements OnInit, OnDestroy {
  @ViewChild(CalendarComponent, { static: true }) calendarComponent!: CalendarComponent;
  @ViewChild("timezoneControl", { static: true }) timezoneControl: TimezoneControlComponent | undefined;
  days: IEventTimeAvailability[] = [];
  day: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  selectedYear: number = 0;
  selectedMonth: number = 0;
  is24HourFormat: boolean = false;
  eventTypeId: string = "";
  timeZoneName: string = "";
  selectedDate: string = "";

  constructor(
    private bookingService: BookingService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.params.subscribe(params => {
      this.eventTypeId = params["id"];
    });
  }

  ngOnInit(): void {
    this.timeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;
    
    this.calendarComponent.resetCalendar();
  }

  ngViewAfterInit() {
  }

  onHandledCalendarClicked(e: any) {
    console.log(`day changed ${e}`);

    if (Object.keys(e).length == 0) return;

    this.selectedDate = Object.keys(e)[Object.keys(e).length - 1];
    this.showAvailableTimeSlotsInDay();

    this.addParamDate();
    //this.day = this.days.find(e => e.date == this.selectedDate);
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
       fromDate = this.selectedYear + "-" + (this.selectedMonth + 1) +"-"+currentDate.getDate();
    }

    this.bookingService.getList(this.eventTypeId, this.timeZoneName, fromDate, toDate).subscribe((data) => {
      this.days = data;
      this.updateTimeLocalTime();
      //this.day = this.days[0];
      this.showAvailableTimeSlotsInDay();

    });
  }

  showAvailableTimeSlotsInDay() {
    this.day = this.days.find(e => e.date == this.selectedDate);
  }
  onLoadedTimezoneData(e: any) {
    console.log(e);
  }
  onChangedTimezone(e: TimeZoneData) {
    console.log(e);
    this.selectedTimeZone = e;
    this.timeZoneName = e.name;
    let localDate = new Date(new Date().toLocaleString("en-US", { timeZone: this.timeZoneName }));
    this.calendarComponent.resetTimeZone(this.timeZoneName);
    //this.calendarComponent.resetCalendar();
    //this.updateTimeLocalTime();
  }

  onChangedHourFormat(is24HourFormat: boolean) {
    this.is24HourFormat = is24HourFormat;
    this.updateTimeLocalTime();
  }

  updateTimeLocalTime() {
    this.days.forEach((day) => {
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
        month: `${this.selectedYear}-${this.selectedMonth}`,
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
  }
}
