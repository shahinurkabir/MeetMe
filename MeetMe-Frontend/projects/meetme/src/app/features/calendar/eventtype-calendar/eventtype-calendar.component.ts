import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalendarComponent, AppointmentService, TimezoneControlComponent, IEventTimeAvailability, TimeZoneData, EventTypeService, IEventType, AccountService, IAccountProfileInfo, ITimeSlot, settings_day_of_week, settings_month_of_year, AlertService, ICreateAppointmentCommand, IEventTypeQuestion, IEventAvailabilityDetailItemDto, IAppointmentQuestionaireItemDto, DateFunction, CommonFunction } from '../../../app-core';
import { Subject, forkJoin, lastValueFrom, takeUntil } from 'rxjs';
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
  timeAvailabilities: IEventTimeAvailability[] = [];
  selectedDates: { [id: string]: string | undefined } = {}
  selectedDayAvailabilities: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  selectedYear: number = 0;
  selectedMonth: number = 0;
  is24HourFormat: boolean = false;
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
  eventTypeQuestions: IEventTypeQuestion[] = [];
  selectedCheckBoxes: any = {};
  formAppoinmentEntry: FormGroup;
  forwardDurationDate?: Date = undefined;

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

  ngOnInit() {
    this.selectedDate = this.route.snapshot.queryParamMap.get('date') ?? "";
    this.selectedYearMonth = this.route.snapshot.queryParamMap.get("month");
    this.eventTypeOwner = this.route.snapshot.paramMap.get("user") ?? "";
    this.event_slug = this.route.snapshot.paramMap.get("event-slug") ?? "";

    this.initializeRouteParameters();
    this.loadEventDetails();
    this.updateCalendar();
    this.loadAvailabilityOutsideOfMonth();

  }
  onClickBack() {

  }
  onCalendarDayClicked(e: any) {

    if (Object.keys(e).length == 0) return;

    this.selectedDate = Object.keys(e)[Object.keys(e).length - 1];
    this.updateAvailableTimeSlotsInDay(this.selectedDate, this.timeAvailabilities);
    this.addParamDate();
  }


  async onMonthChange(e: any) {

    this.selectedYear = e.year;
    this.selectedMonth = e.month;
    this.addParamMonth();

    const dateRange = DateFunction.getFromDateToDate(this.selectedYear, this.selectedMonth, this.selectedTimeZoneName);

    const data = await lastValueFrom(this.eventTypeService.getEventAvailabilityCalendar(this.event_slug, this.selectedTimeZoneName, dateRange.fromDate, dateRange.toDate));

    this.disableCalendarDaysForEmptySlot(dateRange.fromDate, dateRange.toDate, data);

    this.convertToLocalTime(data);

    this.timeAvailabilities = data;

    // in case of calling this method by timezone changed
    if (this.selectedDayAvailabilities)
      this.convertToLocalTime([this.selectedDayAvailabilities])

    if (this.selectedDate && this.isSelectedDateInCurrentMonth(this.selectedDate)) {
      this.updateAvailableTimeSlotsInDay(this.selectedDate, this.timeAvailabilities);
    }
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
    const listRadioButtonsHasRequriedError = this.eventTypeQuestions
      .filter(e => e.questionType == "RadioButtons"
        && e.requiredYN && e.otherOptionYN && !e.otherOptionValue
        && this.formAppoinmentEntry.get('questionResponses')?.get(e.id!)?.value == "Other"
      );

    //validate radio buttons with other option
    if (listRadioButtonsHasRequriedError.length > 0) {
      return;
    }

    const questionnaireFormGroup = this.formAppoinmentEntry.get('questionResponses') as FormGroup;
    const questionnaireFormGroupRowValues = questionnaireFormGroup.getRawValue();

    // to fill other option value in question response
    this.eventTypeQuestions.forEach((question) => {
      if (question.otherOptionYN && question.otherOptionValue) {
        questionnaireFormGroupRowValues[question.id!] = `${questionnaireFormGroupRowValues[question.id!]} - ${question.otherOptionValue}`;
      }
    });
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
    const elementId = element.id.toString();
    const value = element.value;
    let questionId = elementId.indexOf(value) > -1 ? elementId.replace(value, "") : elementId;
    let selectedValue = element.value.toString();

    const questionResponseFormGroup = this.formAppoinmentEntry.get('questionResponses') as FormGroup;
    if (isMultipleChoice) {
      this.handleMultipleChoice(e, questionId, questionResponseFormGroup);
    }
    else {
      questionResponseFormGroup.get(questionId)?.setValue(selectedValue);
    }
  }

  onOtherOptionSelected(e: any, questionItem: IEventTypeQuestion) {
    let element = e.target as HTMLInputElement
    const value = element.value;
    questionItem.otherOptionValue = value;
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


  private async loadAvailabilityOutsideOfMonth() {

    if (this.selectedDate && !this.isSelectedDateInCurrentMonth(this.selectedDate)) {
      const selectedDateTime = new Date(this.selectedDate);
      const dateRange = DateFunction.getFromDateToDate(selectedDateTime.getFullYear(), selectedDateTime.getMonth(), this.selectedTimeZoneName);
      const data = await lastValueFrom(this.eventTypeService.getEventAvailabilityCalendar(this.event_slug, this.selectedTimeZoneName, dateRange.fromDate, dateRange.toDate));
      this.convertToLocalTime(data);
      this.updateAvailableTimeSlotsInDay(this.selectedDate, data);
    }
  }

  private isSelectedDateInCurrentMonth(selectedDate: string): boolean {
    const selectedDateTime = new Date(selectedDate);
    const result = selectedDateTime.getMonth() == this.selectedMonth && selectedDateTime.getFullYear() == this.selectedYear
    return result;
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
      this.accountService.getProfileByUserName(this.eventTypeOwner)
    ])
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.eventTypeInfo = response[0];
          this.eventTypeQuestions = response[0].questions.sort((a, b) => a.displayOrder - b.displayOrder);
          this.eventTypeOwnerInfo = response[1];
          let currentDate = DateFunction.getCurrentDateInTimeZone(this.selectedTimeZoneName);
          if (this.eventTypeInfo?.forwardDurationInDays > 0) {
            this.forwardDurationDate = new Date(currentDate.setDate(this.eventTypeInfo?.forwardDurationInDays));
          }
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



  private disableCalendarDaysForEmptySlot(fromDate: string, toDate: string, availableTimeSlots: IEventTimeAvailability[]) {

    const fromDateObj = new Date(fromDate);
    const toDateObj = new Date(toDate);

    // only disable days in the current month
    if (!this.isSelectedDateInCurrentMonth(fromDate)) return;
    const allDaysInRange = DateFunction.getDaysInRange(fromDateObj, toDateObj);

    if (this.selectedDate) {
      let selectedDayNumber = new Date(this.selectedDate).getDate();
      if (allDaysInRange.filter(e => e == selectedDayNumber).length == 0) {
        this.selectedDayAvailabilities = undefined
      }
    }

    const daysWithAvailability = availableTimeSlots.map((event) => new Date(event.date).getDate());

    // Filter out the days without availability
    const disabledDays = allDaysInRange.filter((day) => !daysWithAvailability.includes(day));

    if (disabledDays.length > 0) {
      this.calendarComponent.disableDays(disabledDays);
    }
  }

  private updateAvailableTimeSlotsInDay(selectedDate: string, timeAvailabilities: IEventTimeAvailability[]) {
    this.selectedDates={[selectedDate]:selectedDate};
    const availabilitiesInSelectedDate = timeAvailabilities.find(e => e.date == selectedDate);

    this.selectedDayAvailabilities = CommonFunction.cloneObject(availabilitiesInSelectedDate);// this.timeAvailabilities.find(e => e.date == this.selectedDate);
  }

  private convertToLocalTime(availableTimeSlots: IEventTimeAvailability[]) {
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
    availableTimeSlots.forEach((day) => {
      day.slots = day.slots.map(convertTimeSlot);
    });

    console.log(availableTimeSlots);
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
