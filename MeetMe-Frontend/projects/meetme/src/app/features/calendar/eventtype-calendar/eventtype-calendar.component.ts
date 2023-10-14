import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarComponent, AppointmentService, TimezoneControlComponent, IEventTimeAvailability, TimeZoneData, EventTypeService, getTimeWithAMPM, IEventType, AccountService, IAccountProfileInfo, ITimeSlot, day_of_week, month_of_year, AlertService, ICreateAppointmentCommand, convertTimeZone, getDaysInMonth, getDateString, getCurrentDateInTimeZone, getDaysInRange } from '../../../app-core';
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

    this.initializeRouteParameters();
    this.loadEventDetails();
    this.updateCalendar();

  }

  onHandledCalendarClicked(e: any) {
    console.log(`day changed ${e}`);

    if (Object.keys(e).length == 0) return;

    this.selectedDate = Object.keys(e)[Object.keys(e).length - 1];
    this.showAvailableTimeSlotsInDay();

    this.addParamDate();
  }

  onHandledMonthChange(e: any) {

    this.selectedYear = e.year;
    this.selectedMonth = e.month;
    this.addParamMonth();
    this.loadEventAvailabileTimeSlots();
  }

  onLoadedTimezoneData(e: any) {
    console.log(e);
  }
  onChangedTimezone(e: TimeZoneData) {
    console.log(e);
    this.selectedTimeZone = e;
    this.selectedTimeZoneName = e.name;
    this.calendarComponent.resetTimeZone(this.selectedTimeZoneName);

  }

  onChangedHourFormat(is24HourFormat: boolean) {
    this.is24HourFormat = is24HourFormat;
    this.updateTimeLocalTime();
  }

  onSelectedTimeSlot(e: ITimeSlot) {
    this.selectedTimeSlot = e;
    let fromTime = e.startTime;
    let toTime = getTimeWithAMPM(new Date(new Date(e.startDateTime).setMinutes(this.eventTypeInfo?.duration!)), this.is24HourFormat, this.selectedTimeZoneName!);
    let day = new Date(e.startDateTime).getDate();
    let weekDay = new Date(e.startDateTime).getDay();
    let dayOfWeek = day_of_week[weekDay];
    let monthName = month_of_year[new Date(e.startDateTime).getMonth()];
    let year = new Date(e.startDateTime).getFullYear();
    this.selectedDateTime = `${fromTime} - ${toTime}, ${dayOfWeek},${monthName} ${day}, ${year}`;

  }

  onSubmitAppointmentForm(form: NgForm) {

    this.submitted = true;

    if (form.invalid) {
      return;
    }

    // crate the command object with the data from the form
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

    //show the loading indicator
    this.isSubmitting = true;

    // call the calendar service to create the appointment
    this.calendarService
      .addAppointment(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          // handle the successfull response
          console.log(response);
          this.apointmentCreated(response);
          this.alertService.success("Appointment created successfully");
        }
        ,
        error: (error) => {
          //handle the error response
          console.log(error)
        },
        complete: () => {
          // hide the loading indicator when the call is completed
          this.isSubmitting = false;
        }
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

  private apointmentCreated(appointmentId: string) {
    this.router.navigate(["calendar/booking", this.eventTypeOwner, this.eventTypeInfo?.slug, appointmentId, "view"]);
  }

  private initializeRouteParameters(): void {
    this.eventTypeId = this.route.snapshot.queryParamMap.get("id") || "";
    this.selectedDate = this.route.snapshot.queryParamMap.get('date') || "";
    this.selectedYearMonth = this.route.snapshot.queryParamMap.get("month") || "";
    this.eventTypeOwner = this.route.snapshot.paramMap.get("user") || "";
  }

  private updateCalendar(): void {
    if (this.selectedYearMonth) {
      const [year, month] = this.selectedYearMonth.split("-").map(Number);
      this.selectedYear = year;
      this.selectedMonth = month - 1;

      const selectedDatesFromCalendar = this.selectedDate ? { [this.selectedDate]: this.selectedDate } : {};

      this.calendarComponent.moveTo(this.selectedYear, this.selectedMonth, selectedDatesFromCalendar);
    } else {
      this.calendarComponent.resetCalendar();
    }
  }


  private loadEventDetails() {
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

  private loadEventAvailabileTimeSlots() {

    const daysInMonth = getDaysInMonth(this.selectedYear, this.selectedMonth);

    let fromDate = getDateString(this.selectedYear, this.selectedMonth, 1);
    const toDate = getDateString(this.selectedYear, this.selectedMonth, daysInMonth);

    let currentDate = getCurrentDateInTimeZone(this.selectedTimeZoneName);
    let isCurrentMonth = currentDate.getMonth() == this.selectedMonth && currentDate.getFullYear() == this.selectedYear;

    if (isCurrentMonth) {
      fromDate = getDateString(this.selectedYear, this.selectedMonth, currentDate.getDate());
    }

    this.fetchCalendarAvailability(fromDate, toDate);

    // this.eventTypeService.getCalendarAvailability(this.eventTypeId, this.selectedTimeZoneName, fromDate, toDate)
    //   .pipe(takeUntil(this.destroyed$))
    //   .subscribe({
    //     next: (response) => {
    //       this.availableTimeSlots = response;
    //       this.updateTimeLocalTime();
    //       this.disableNotAvailableDays(fromDate, toDate, response);
    //       this.showAvailableTimeSlotsInDay();
    //     },
    //     error: (error) => { console.log(error) },
    //     complete: () => { }
    //   });
  }
  private fetchCalendarAvailability(fromDate: string, toDate: string): void {
    this.eventTypeService
      .getEventAvailabilityCalendar(this.eventTypeId, this.selectedTimeZoneName, fromDate, toDate)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.availableTimeSlots = response;
          this.updateTimeLocalTime();
          this.disableCalendarDaysForEmptySlot(fromDate, toDate, response);
          //this.showAvailableTimeSlotsInDay();
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {
          // Optionally, you can handle completion here.
        },
      });
  }

  private disableCalendarDaysForEmptySlot(fromDate: string, toDate: string, availableTimeSlots: IEventTimeAvailability[]) {

    const fromDateObj = new Date(fromDate);
    const toDateObj = new Date(toDate);

    // Create an array of all days within the date range
    const allDaysInRange = getDaysInRange(fromDateObj, toDateObj);

    // Extract the day numbers from availableTimeSlots
    const daysWithAvailability = availableTimeSlots.map((event) => new Date(event.date).getDate());

    // Filter out the days without availability
    const disabledDays = allDaysInRange.filter((day) => !daysWithAvailability.includes(day));

    if (disabledDays.length > 0) {
      this.calendarComponent.disableDays(disabledDays);
    }
  }

  private showAvailableTimeSlotsInDay() {
    this.day = this.availableTimeSlots.find(e => e.date == this.selectedDate);
  }
  private updateTimeLocalTime() {
    // Function to convert a single time slot
    const convertTimeSlot = (slot: ITimeSlot) => {
      const startDateTime = convertTimeZone(slot.startDateTime, this.selectedTimeZone?.name!);
      const timeToStart = getTimeWithAMPM(new Date(slot.startDateTime), this.is24HourFormat, this.selectedTimeZone?.name!);
      const hourOfDay = startDateTime.getHours();
      const isDaySecondHalf = timeToStart.includes('PM') || (hourOfDay >= 12 && hourOfDay < 24);

      return {
        ...slot,
        startTime: timeToStart,
        isDaySecondHalf: isDaySecondHalf,
      };
    };

    // Update time slots for each day
    this.availableTimeSlots.forEach((day) => {
      day.slots = day.slots.map(convertTimeSlot);
    });
  }

  private addParamMonth() {
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
  private addParamDate() {
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