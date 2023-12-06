import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarComponent, AppointmentService, TimezoneControlComponent, IEventTimeAvailability, TimeZoneData, EventTypeService, IEventType, AccountService, IAccountProfileInfo, ITimeSlot, settings_day_of_week, settings_month_of_year, AlertService, ICreateAppointmentCommand, IEventTypeQuestion, IEventAvailabilityDetailItemDto, IAppointmentQuestionaireItemDto, DateFunction } from '../../../app-core';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { FormArray, FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';

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
  selectedDayAvailabilities: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  selectedYear: number = 0;
  selectedMonth: number = 0;
  is24HourFormat: boolean = false;
  //eventTypeId: string = "";
  event_slug: string = "";
  selectedTimeZoneName: string = "";
  selectedDate: string | null = null;
  selectedYearMonth: string | null = null;
  eventTypeInfo: IEventType | undefined;
  eventTypeOwner: string = "";
  eventTypeOwnerInfo: IAccountProfileInfo | undefined;
  selectedDateTime: string = "";
  selectedTimeSlot: ITimeSlot | undefined;
  submitted: boolean = false;
  isSubmitting: boolean = false;
  isAppointmentCreated: boolean = false;
  //eventTypeAvailability: IEventAvailabilityDetailItemDto[] = [];
  eventTypeQuestions: IEventTypeQuestion[] = [];
  selectedCheckBoxes: any = {};
  formAppoinmentEntry: FormGroup;

  constructor(
    private eventTypeService: EventTypeService,
    private calendarService: AppointmentService,
    private accountService: AccountService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {

    this.formAppoinmentEntry = this.formBuilder.group({
      inviteeName: ['', Validators.required],
      inviteeEmail: ['', Validators.required],
      guestEmails: [''],
      note: [''],
      questionResponses: new FormGroup({})
    });

    this.selectedTimeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;

  }

  ngOnInit(): void {
    //this.eventTypeId = this.route.snapshot.queryParamMap.get("id") ?? "";
    this.selectedDate = this.route.snapshot.queryParamMap.get('date') ?? "";
    this.selectedYearMonth = this.route.snapshot.queryParamMap.get("month");
    this.eventTypeOwner = this.route.snapshot.paramMap.get("user") ?? "";
    this.event_slug = this.route.snapshot.paramMap.get("event-slug") ?? "";

    this.initializeRouteParameters();
    this.loadEventDetails();
    this.updateCalendar();

  }
  onClickBack() {

  }
  onCalendarDayClicked(e: any) {

    if (Object.keys(e).length == 0) return;

    this.selectedDate = Object.keys(e)[Object.keys(e).length - 1];

    this.showAvailableTimeSlotsInDay();

    this.addParamDate();
  }

  onMonthChange(e: any) {

    this.selectedYear = e.year;
    this.selectedMonth = e.month;
    this.addParamMonth();
    this.loadCalendarTimeSlots();
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

    const startTime: string = e.startTime;
    const endTime: string = this.calculateEndTime(
      e.startDateTime,
      this.eventTypeInfo?.duration!,
      this.is24HourFormat,
      this.selectedTimeZoneName!
    );

    const date: Date = new Date(e.startDateTime);
    const day: number = date.getDate();
    const weekDay: number = date.getDay();
    const dayOfWeek: string = settings_day_of_week[weekDay];
    const monthName: string = settings_month_of_year[date.getMonth()];
    const year: number = date.getFullYear();

    this.selectedDateTime = `${startTime} - ${endTime}, ${dayOfWeek}, ${monthName} ${day}, ${year}`;

  }

  onSubmitAppointmentForm() {

    this.submitted = true;
    console.log(this.formAppoinmentEntry.value);

    if (this.formAppoinmentEntry.invalid) {
      return;
    }

    const questionnaireFormGroup = this.formAppoinmentEntry.get('questionResponses') as FormGroup;
    const questionnaireFormGroupRowValues = questionnaireFormGroup.getRawValue();
    const questionnaireResponseContent = this.getQuestionnaireResponseContent(questionnaireFormGroupRowValues);

    let command: ICreateAppointmentCommand = {
      eventTypeId: this.eventTypeInfo?.id!,
      inviteeName: this.formAppoinmentEntry.get('inviteeName')?.value,
      inviteeEmail: this.formAppoinmentEntry.get('inviteeEmail')?.value,
      inviteeTimeZone: this.selectedTimeZoneName!,
      startTime: this.selectedTimeSlot?.startDateTime!,
      meetingDuration: this.eventTypeInfo?.duration!,
      guestEmails: this.formAppoinmentEntry.get('guestEmails')?.value,
      note: this.formAppoinmentEntry.get('note')?.value,
      questionnaireContent: questionnaireResponseContent
    };

    this.isSubmitting = true;

    this.calendarService
      .addAppointment(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.appointmentCreatedResponse(response);
          this.alertService.success("Appointment created successfully");
        },
        error: (error) => {
          console.log(error)
        },
        complete: () => {
          this.isSubmitting = false;
        }
      });
  }

  onAnswerSelectionChnaged(e: any, isMultipleChoice: boolean = false) {
    let element = e.target as HTMLInputElement
    let questionId = element.id.toString();
    let selectedValue = element.value.toString();

    const questionResponseFormGroup = this.formAppoinmentEntry.get('questionResponses') as FormGroup;
    if (isMultipleChoice) {
      this.handleMultipleChoice(e, questionId, questionResponseFormGroup);
    }
    else {
      questionResponseFormGroup.get(questionId)?.setValue(selectedValue);
    }
  }


  onCancelAppointmentEntry(e: any) {
    e.preventDefault();
    this.selectedTimeSlot = undefined;
    this.selectedDateTime = "";
    this.resetQuestionnairesForm();
    this.formAppoinmentEntry?.reset();
    this.formAppoinmentEntry.markAsPristine();
    this.submitted = false;
  }

  private getQuestionnaireResponseContent(questionResponses: any): string | undefined {

    const questionoareResponseItemDtos: IAppointmentQuestionaireItemDto[] = [];

    let content = undefined;

    if (!questionResponses) return content;

    for (const [key, value] of Object.entries(questionResponses)) {

      if (!value) continue;

      const isArrayResponse = Array.isArray(value);
      if (isArrayResponse && value.length == 0) continue;

      const response = isArrayResponse ? (value as string[]).join(" , ") : (value as string).toString();
      const questionName = this.eventTypeQuestions.find(e => e.id == key)?.name!;
      questionoareResponseItemDtos.push({ questionId: key, questionName: questionName, answer: response, isMultipleChoice: isArrayResponse });

    }

    if (questionoareResponseItemDtos.length > 0) {
      content = JSON.stringify(questionoareResponseItemDtos);
    }

    return content;

  }
  private appointmentCreatedResponse(appointmentId: string) {
    this.router.navigate(["calendar/booking", this.eventTypeOwner, this.eventTypeInfo?.slug, appointmentId, "view"]);
  }

  private initializeRouteParameters(): void {
    // this.eventTypeId = this.route.snapshot.queryParamMap.get("id") || "";
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

  get f() {
    return this.formAppoinmentEntry.controls;
  }
  get dynamicFields() {
    let group = this.formAppoinmentEntry?.get('questionResponses') as FormGroup
    return group.controls;
  }
  private calculateEndTime(startDateTime: string, duration: number, is24HourFormat: boolean, selectedTimeZoneName: string): string {
    const endDate: Date = new Date(startDateTime);
    endDate.setMinutes(endDate.getMinutes() + duration);

    return DateFunction.getTimeWithAMPM(endDate, is24HourFormat, selectedTimeZoneName);
  }

  private handleMultipleChoice(e: any, questionName: string, formGroup: FormGroup) {

    const selectedValue = e.target.value;
    const selectedCheckBoxesControlArray = formGroup.get(questionName) as FormArray;

    if (e.target.checked) {
      selectedCheckBoxesControlArray.push(new FormControl(selectedValue));
    }
    else {
      let i: number = 0;
      selectedCheckBoxesControlArray.controls.forEach((item: any) => {
        if (item.value == e.target.value) {
          selectedCheckBoxesControlArray.removeAt(i);
          return;
        }
        i++;
      });
    }

  }

  private loadEventDetails() {
    forkJoin([
      this.eventTypeService.getBySlugName(this.event_slug),
      //this.eventTypeService.getEventAvailability(this.eventTypeId),
      //this.eventTypeService.getEventQuetions(this.eventTypeId),
      this.accountService.getProfileByUserName(this.eventTypeOwner)
    ])
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.eventTypeInfo = response[0];
          //this.eventTypeAvailability = response[1];
          this.eventTypeQuestions = response[0].questions;
          this.eventTypeOwnerInfo = response[1];
        },
        error: (error) => { console.log(error) },
        complete: () => {

          this.resetQuestionnairesForm();
        }
      });
  }

  private resetQuestionnairesForm() {
    let group = this.formAppoinmentEntry?.get('questionResponses') as FormGroup
    group.reset();
    while (Object.keys(group.controls).length) {
      const toRemove = Object.keys(group.controls)[0];
      group.removeControl(toRemove)
    }

    if (this.eventTypeQuestions.length || this.eventTypeQuestions.length == 0) {
      const questionResponsesFormGroup = this.formAppoinmentEntry?.get('questionResponses') as FormGroup;
      this.eventTypeQuestions.forEach((question) => {
        if (question.questionType == "CheckBoxes") {
          questionResponsesFormGroup?.addControl(question.id!, this.formBuilder.array([], question.requiredYN ? Validators.required : null));
        }
        else {
          questionResponsesFormGroup?.addControl(question.id!, this.formBuilder.control('', question.requiredYN ? Validators.required : null));
        }
      });
    }
  }

  private loadCalendarTimeSlots() {

    const daysInMonth = DateFunction.getDaysInMonth(this.selectedYear, this.selectedMonth);

    let fromDate = DateFunction.getDateString(this.selectedYear, this.selectedMonth, 1);
    const toDate = DateFunction.getDateString(this.selectedYear, this.selectedMonth, daysInMonth);

    let currentDate = DateFunction.getCurrentDateInTimeZone(this.selectedTimeZoneName);
    let isCurrentMonth = currentDate.getMonth() == this.selectedMonth && currentDate.getFullYear() == this.selectedYear;

    if (isCurrentMonth) {
      fromDate = DateFunction.getDateString(this.selectedYear, this.selectedMonth, currentDate.getDate());
    }

    this.fetchCalendarAvailability(fromDate, toDate);

  }

  private fetchCalendarAvailability(fromDate: string, toDate: string): void {
    this.eventTypeService
      .getEventAvailabilityCalendar(this.event_slug, this.selectedTimeZoneName, fromDate, toDate)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.availableTimeSlots = response;
          this.updateTimeLocalTime();
          this.disableCalendarDaysForEmptySlot(fromDate, toDate, response);
          this.showAvailableTimeSlotsInDay();
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {
        },
      });
  }

  private disableCalendarDaysForEmptySlot(fromDate: string, toDate: string, availableTimeSlots: IEventTimeAvailability[]) {

    const fromDateObj = new Date(fromDate);
    const toDateObj = new Date(toDate);

    const allDaysInRange = DateFunction.getDaysInRange(fromDateObj, toDateObj);

    const daysWithAvailability = availableTimeSlots.map((event) => new Date(event.date).getDate());

    // Filter out the days without availability
    const disabledDays = allDaysInRange.filter((day) => !daysWithAvailability.includes(day));

    if (disabledDays.length > 0) {
      this.calendarComponent.disableDays(disabledDays);
    }
  }

  private showAvailableTimeSlotsInDay() {

    if (!this.selectedDate) return;

    const earlierDate = new Date(this.selectedDate);
    if (earlierDate.getMonth() != this.selectedMonth || earlierDate.getFullYear() != this.selectedYear) return;

    this.selectedDayAvailabilities = this.availableTimeSlots.find(e => e.date == this.selectedDate);
  }

  private updateTimeLocalTime() {
    // Function to convert a single time slot
    const convertTimeSlot = (slot: ITimeSlot) => {
      const startDateTime = DateFunction.convertTimeZone(slot.startDateTime, this.selectedTimeZone?.name!);
      const timeToStart = DateFunction.getTimeWithAMPM(new Date(slot.startDateTime), this.is24HourFormat, this.selectedTimeZone?.name!);
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

    console.log(this.availableTimeSlots);
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
