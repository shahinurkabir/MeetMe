import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarComponent, AppointmentService, TimezoneControlComponent, IEventTimeAvailability, TimeZoneData, EventTypeService, convertTimeZoneLocalTime, IEventType, AccountService, IAccountProfileInfo, ITimeSlot, day_of_week, month_of_year, AlertService, ICreateAppointmentCommand } from '../../../app-core';
import { Subject, forkJoin, from, takeUntil } from 'rxjs';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-event-type-calendar',
  templateUrl: './eventtype-calendar.component.html',
  styleUrls: ['./eventtype-calendar.component.scss']
})
export class EventTypeCalendarComponent implements OnInit, OnDestroy {
  @ViewChild(CalendarComponent, { static: true }) calendarComponent!: CalendarComponent;
  @ViewChild("timezoneControl", { static: true }) timezoneControl: TimezoneControlComponent | undefined;
  @ViewChild("appointmentForm") appointmentForm: NgForm | undefined;
  destroyed$: Subject<boolean> = new Subject<boolean>();
  availableTimeSlots: IEventTimeAvailability[] = [];
  day: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  selectedYear: number = 0;
  selectedMonth: number = 0;
  is24HourFormat: boolean = false;
  eventTypeId: string = "";
  selectedTimeZoneName: string = "";
  selectedDate: string | null = null;
  selectedYearMonth: string | null = null;
  eventTypeInfo: IEventType | undefined;
  eventTypeOwner: string = "";
  eventTypeOwnerInfo: IAccountProfileInfo | undefined;
  selectedDateTime: string = "";
  selectedTimeSlot: ITimeSlot | undefined;
  appointmentEntryModel: AppointmentEntryModel = {
    inviteeName: "",
    inviteeEmail: "",
    guestEmails: "",
    note: ""

  };
  submitted: boolean = false;
  isSubmitting: boolean = false;
  isAppointmentCreated: boolean = false;
  constructor(
    private eventTypeService: EventTypeService,
    private calendarService: AppointmentService,
    private accountService: AccountService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.selectedTimeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;

  }

  ngOnInit(): void {
    this.eventTypeId = this.route.snapshot.queryParamMap.get("id") ?? "";
    this.selectedDate = this.route.snapshot.queryParamMap.get('date');
    this.selectedYearMonth = this.route.snapshot.queryParamMap.get("month");
    this.eventTypeOwner = this.route.snapshot.paramMap.get("user") ?? "";

    this.loadEventTypeAndUserInfo();

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

  loadEventTypeAndUserInfo() {
    forkJoin([
      this.eventTypeService.getById(this.eventTypeId),
      this.accountService.getProfileByUserName(this.eventTypeOwner)
    ])
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.eventTypeInfo = response[0];
          this.eventTypeOwnerInfo = response[1];
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
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

    let currentDate = new Date(new Date().toLocaleString("en-US", { timeZone: this.selectedTimeZoneName }));
    let isCurrentMonth = currentDate.getMonth() == this.selectedMonth && currentDate.getFullYear() == this.selectedYear;
    if (isCurrentMonth) {
      fromDate = this.selectedYear + "-" + (this.selectedMonth + 1) + "-" + currentDate.getDate();
    }

    this.eventTypeService.getCalendarAvailability(this.eventTypeId, this.selectedTimeZoneName, fromDate, toDate)
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
    this.selectedTimeZoneName = e.name;
    this.calendarComponent.resetTimeZone(this.selectedTimeZoneName);
    //this.showAvailableTimeSlotsInDay();
    //this.updateTimeLocalTime();
    this.loadEventCalendarData()
  }

  onChangedHourFormat(is24HourFormat: boolean) {
    this.is24HourFormat = is24HourFormat;
    this.updateTimeLocalTime();
  }

  updateTimeLocalTime() {
    this.availableTimeSlots.forEach((day) => {
      day.slots.forEach((slot) => {
        let timeToStart = convertTimeZoneLocalTime(new Date(slot.startDateTime), this.is24HourFormat, this.selectedTimeZone?.name!);
        let isDaySecondHalf = (timeToStart.split(" ")[1] == "PM" || parseInt(timeToStart.substring(0, 2)) >= 12) ? true : false;
        slot.startTime = timeToStart;
        slot.isDaySecondHalf = isDaySecondHalf
      });
    });
  }
  onSelectedTimeSlot(e: ITimeSlot) {
    this.selectedTimeSlot = e;
    let fromTime = e.startTime;
    let toTime = convertTimeZoneLocalTime(new Date(new Date(e.startDateTime).setMinutes(this.eventTypeInfo?.duration!)), this.is24HourFormat, this.selectedTimeZoneName!);
    let day = new Date(e.startDateTime).getDate();
    let weekDay = new Date(e.startDateTime).getDay();
    let dayOfWeek = day_of_week[weekDay];
    let monthName = month_of_year[new Date(e.startDateTime).getMonth()];
    let year = new Date(e.startDateTime).getFullYear();
    this.selectedDateTime = `${fromTime} - ${toTime}, ${dayOfWeek},${monthName} ${day}, ${year}`;

  }

  onSubmitAppointmentForm(form: NgForm) {
    this.submitted = true;
    if (form.invalid) return;

    let command: ICreateAppointmentCommand = {
      eventTypeId: this.eventTypeId,
      inviteeName: this.appointmentEntryModel?.inviteeName!,
      inviteeEmail: this.appointmentEntryModel?.inviteeEmail!,
      inviteeTimeZone: this.selectedTimeZoneName!,
      startTime: this.selectedTimeSlot?.startDateTime!,
      meetingDuration: this.eventTypeInfo?.duration!,
      guestEmails: this.appointmentEntryModel?.guestEmails,
      note: this.appointmentEntryModel?.note
    };

    this.isSubmitting = true;
    this.calendarService.addAppointment(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.alertService.success("Appointment created successfully");
        }
        ,
        error: (error) => { console.log(error) },
        complete: () => { this.isSubmitting = false; }
      });
  }

  onCancelBooking(e: any) {
    e.preventDefault();
    this.selectedTimeSlot = undefined;
    this.selectedDateTime = "";
    this.appointmentForm?.resetForm();
    this.appointmentForm?.form.markAsPristine();
    this.submitted = false;
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

interface AppointmentEntryModel {
  inviteeName: string
  inviteeEmail: string
  guestEmails?: string
  note?: string
}