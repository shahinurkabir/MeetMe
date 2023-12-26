import { Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AlertService, AppointmentService,  CommonFunction, EventTypeService, IAppointmentDetailsDto, IAppointmentSearchParametersDto, IAppointmentsPaginationResult, ICancelAppointmentCommand, IDay, IEventType, MultiCalendarComponent, settings_appointment_search_by_date_option, settings_appointment_status } from '../../../app-core';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent implements OnInit, OnDestroy {
  @ViewChild('toggleButtonDateRangeSelectionWindow') toggleButtonDateRangeSelectionWindow: ElementRef | undefined
  @ViewChild('dateRangeSelectionWindow') dateRangeSelectionWindow: ElementRef | undefined

  @ViewChild('toggleButtonEventTypesFilterWindow') toggleButtonEventTypesFilterWindow: ElementRef | undefined
  @ViewChild('eventTypesFilterWindow') eventTypesFilterWindow: ElementRef | undefined

  @ViewChild('toggleButtonAppointmentStatusByFilterWindow') toggleButtonAppointmentStatusByFilterWindow: ElementRef | undefined
  @ViewChild('appointmentStatusByFilterWindow') appointmentStatusByFilterWindow: ElementRef | undefined


  @ViewChild('toggleButtonInviteeEmailByFilterWindow') toggleButtonInviteeEmailByFilterWindow: ElementRef | undefined
  @ViewChild('inviteeEmailByFilterWindow') inviteeEmailByFilterWindow: ElementRef | undefined


  @ViewChild("multiCalendar") multiCalendarComponent: MultiCalendarComponent | undefined;

  destroyed$: Subject<boolean> = new Subject<boolean>();

  appointmentsPaginationResult: IAppointmentsPaginationResult | undefined;
  cancellationReason: string = '';
  timeZoneName: string = Intl.DateTimeFormat().resolvedOptions().timeZone;
  eventTypesList: IEventType[] = [];

  selectedEventTypeIds: string[] = [];
  searchByDateOption: string = settings_appointment_search_by_date_option.upcoming;
  currentPageNumber: number = 1;
  filterByEventTypeYN: boolean = false;
  filterByAppointmentStatusYN: boolean = false;
  filterByInviteeEmailYN: boolean = false;
  filterByEventTypeText: string = 'All Event Types';
  filterByStatusText: string = 'Active Events';
  filterByInviteeEmailText: string = 'All Invitee Emails';

  selectedAppointmentStatuses: string[] = [settings_appointment_status.active];
  selectedInviteeEmails: string[] = [];
  statusListFilterForm!: FormGroup;
  entityTypesFilterForm!: FormGroup;
  inviteeEmailsFilterForm!: FormGroup;
  showFilterMenu: boolean = false;
  showDateSelectionWidget: boolean = false;

  selectedDateRanges: { [id: string]: IDay } = {};
  dateRange1: Date = new Date(new Date().toISOString().split('T')[0]);
  dateRange2: Date = new Date(new Date().toISOString().split('T')[0]);
  showCancelAppointmentWindowYN: boolean = false;
  selectedAppointmentItem: IAppointmentDetailsDto | undefined;
  selectedDateRangeText: string = '';

  constructor(
    private appointmentService: AppointmentService,
    private eventTypeService: EventTypeService,
    private renderer: Renderer2,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
  ) {

    this.renderer.listen('window', 'click', (e: Event) => {

      if (e.target != this.toggleButtonDateRangeSelectionWindow?.nativeElement &&
        !this.dateRangeSelectionWindow?.nativeElement?.contains(e.target)) {
        this.showDateSelectionWidget = false;
      }

      if (
        e.target != this.toggleButtonEventTypesFilterWindow?.nativeElement &&
        !this.eventTypesFilterWindow?.nativeElement?.contains(e.target)
      ) {
        this.filterByEventTypeYN = false;
      }

      if (
        e.target != this.toggleButtonAppointmentStatusByFilterWindow?.nativeElement &&
        !this.appointmentStatusByFilterWindow?.nativeElement.contains(e.target)
      ) {
        this.filterByAppointmentStatusYN = false;
      }

      if (e.target != this.toggleButtonInviteeEmailByFilterWindow?.nativeElement &&
        !this.inviteeEmailByFilterWindow?.nativeElement?.contains(e.target)) {
        this.filterByInviteeEmailYN = false;
      }

    });
  }

  ngOnInit(): void {
    this.configureStatusFilterForm();
    this.resetData();
    this.loadData();
  }
  initDateRange() {
    this.selectedDateRanges = {};
    this.selectedDateRangeText = '';
  }
  onToggleAppointmentDetails(item: IAppointmentDetailsDto): void {
    item.isExpanded = !item.isExpanded;
  }

  onToggleCancelAppointmentWindow(item: IAppointmentDetailsDto | undefined): void {
    this.showCancelAppointmentWindowYN = !this.showCancelAppointmentWindowYN;
    this.selectedAppointmentItem = item;
  }

  onEventTypesLoaded(eventTypes: IEventType[]) {
    this.eventTypesList = eventTypes;
    this.configureEntityTypesFilterForm();
  }

  onFilterDateOptionClick(filterDateOption: string) {

    if (this.searchByDateOption == filterDateOption) return;

    this.searchByDateOption = filterDateOption;
    this.currentPageNumber = 1;
    this.initDateRange();
    this.filterAppointments();

  }

  onFilterByDateRangeClick() {
    this.showDateSelectionWidget = !this.showDateSelectionWidget;

    if (!this.showDateSelectionWidget) return;
    this.multiCalendarComponent?.resetDates();
    
  }
  onClickFilterByEvenTypeId(eventTypeId: string) {
    this.selectedEventTypeIds = [];
    this.currentPageNumber = 1;
    this.showFilterMenu = true;
    this.selectedEventTypeIds.push(eventTypeId);
    this.updateFilterFieldsText();
    this.filterAppointments();
  }
  onCancelAppointment() {
    let cancelAppointmentCommand: ICancelAppointmentCommand = {
      id: this.selectedAppointmentItem?.id!,
      cancellationReason: this.selectedAppointmentItem?.cancellationReason!
    }

    this.appointmentService.cancelAppointment(cancelAppointmentCommand)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.showCancelAppointmentWindowYN = false;
          this.alertService.success("Appointment cancelled successfully");
          
          this.filterAppointments();
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {
        }
      })
  }
  onRescheduleAppointment(id: string) {
  }

  onToggleEventTypesFilterWindow(e: any) {
    e.preventDefault();
    this.filterByEventTypeYN = !this.filterByEventTypeYN;
    if (this.filterByEventTypeYN) {
      this.configureEntityTypesFilterForm();
    }
  }

  onEventTypeApplyFilter() {
    this.filterByEventTypeYN = false;
    this.selectedEventTypeIds = [];
    this.currentPageNumber = 1;
    this.entityTypesFilterForm.value.entityTypesList.forEach((item: any) => {
      if (item.isSelected) {
        this.selectedEventTypeIds.push(item.entityValue);
      }
    });

    this.updateFilterFieldsText();

    this.filterAppointments();

  }
  onEventTypeCancelFilter() {
    this.filterByEventTypeYN = false;
  }

  onToggleAppointmentStatusFilterWindow(e: any) {
    e.preventDefault();
    this.filterByAppointmentStatusYN = !this.filterByAppointmentStatusYN;
    if (this.filterByAppointmentStatusYN) {
      this.configureStatusFilterForm();
    }
  }

  onApplyStatusFilter() {
    this.filterByAppointmentStatusYN = false;
    this.selectedAppointmentStatuses = [];
    this.currentPageNumber = 1;
    this.statusListFilterForm.value.statusList.forEach((item: any) => {
      if (item.isSelected) {
        this.selectedAppointmentStatuses.push(item.statusValue);
      }
    });
    this.updateFilterFieldsText();
    this.filterAppointments();


  }
  onCancelStatusFilter() {
    this.filterByAppointmentStatusYN = false;
  }

  onToggleInviteeEmailFilterWindow() {
    this.filterByInviteeEmailYN = !this.filterByInviteeEmailYN;

  }

  onApplyInviteeEmailFilter() {
    this.filterByInviteeEmailYN = false;

    this.updateFilterFieldsText();
    this.filterAppointments();


  }
  onCancelInviteeEmailFilter() {
    this.filterByInviteeEmailYN = false;
  }

  onClearFilter() {
    this.selectedEventTypeIds = [];
    this.selectedAppointmentStatuses = [settings_appointment_status.active];
    this.updateFilterFieldsText();
    this.filterAppointments();
  }
  onToggleFilterMenu() {
    this.showFilterMenu = !this.showFilterMenu;
  }
  onPagerLinkClick(pageNumber: number) {
    this.currentPageNumber = pageNumber;
    this.filterAppointments();
  }

  onCancelDateRangeFilter() {
    this.showDateSelectionWidget = false;
  }

  onSelectedDatesChanged(selecteDates: { [id: string]: IDay }) {
    this.showDateSelectionWidget = false;
    this.searchByDateOption = settings_appointment_search_by_date_option.daterange;
    this.currentPageNumber = 1;
    this.selectedDateRanges = {...selecteDates};
    let dateRangeKeys = Object.keys(this.selectedDateRanges);
    this.selectedDateRangeText = `${this.selectedDateRanges[dateRangeKeys[0]].date} - ${this.selectedDateRanges[dateRangeKeys[1]].date}`;
    this.filterAppointments();
  }
  onDatePeriodChanged(period: string) {
    this.showDateSelectionWidget = false;
    this.searchByDateOption = period;
    this.selectedDateRangeText = CommonFunction.capitalizeFirstLetter(period.replace('_', ' '));
    if (period == 'alltime') {
      this.initDateRange();
      this.searchByDateOption = settings_appointment_search_by_date_option.upcoming;
    }
    this.currentPageNumber = 1;
    this.filterAppointments();
  }
  onCancelDateSelection() {
    this.showDateSelectionWidget = false;
  }
  private updateFilterFieldsText() {

    if (this.selectedEventTypeIds.length == 0) {
      this.filterByEventTypeText = 'All Event Types';
    }
    else {
      this.filterByEventTypeText = this.selectedEventTypeIds.length == 1 ? '1 Event Type' : this.selectedEventTypeIds.length + ' Event Types';
    }
    this.filterByStatusText = this.selectedAppointmentStatuses.length == 1 ? CommonFunction.capitalizeFirstLetter(this.selectedAppointmentStatuses[0]) + " Event" : 'All Events';

  }

  private addQueryParamsToUrl(obj: IAppointmentSearchParametersDto) {
    const queryParams = CommonFunction.generateQueryParamsFromObject(obj);

    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: queryParams,
      queryParamsHandling: ''
    });

  }

  private loadData() {
    const appointSearchParameters: IAppointmentSearchParametersDto = {
      period: this.searchByDateOption,
      statusNames: this.selectedAppointmentStatuses,
      timeZone: this.timeZoneName,
      inviteeEmail: '',
      eventTypeIds: [],
      pageNumber: this.currentPageNumber
    };

    const queryParams = CommonFunction.convertToUriEncodedString(appointSearchParameters);
    forkJoin([
      this.eventTypeService.getList(),
      this.appointmentService.getScheduleEvents(queryParams)
    ])
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.onEventTypesLoaded(response[0]);
          this.appointmentsPaginationResult = response[1];
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {

        }
      })
  }

  private configureStatusFilterForm() {
    const statusListGroup = [
      this.fb.group({
        statusText: CommonFunction.capitalizeFirstLetter(settings_appointment_status.active),
        statusValue: settings_appointment_status.active,
        isSelected: this.selectedAppointmentStatuses.findIndex((item) => item == settings_appointment_status.active) != -1 ? true : false
      }),
      this.fb.group({
        statusText: CommonFunction.capitalizeFirstLetter(settings_appointment_status.cancelled),
        statusValue: settings_appointment_status.cancelled,
        isSelected: this.selectedAppointmentStatuses.findIndex((item) => item == settings_appointment_status.cancelled) != -1 ? true : false
      })
    ];

    this.statusListFilterForm = this.fb.group({
      statusList: this.fb.array(statusListGroup || [])
    });
  }

  private configureEntityTypesFilterForm() {
    let entityTypesListGroup =
      this.eventTypesList.map((item) => {
        return this.fb.group({
          entityText: item.name,
          entityValue: item.id,
          isSelected: this.selectedEventTypeIds.findIndex((selectedItem) => selectedItem == item.id) != -1 ? true : false
        })
      });

    this.entityTypesFilterForm = this.fb.group({
      entityTypesList: this.fb.array(entityTypesListGroup || [])
    });
  }

  private resetData() {
    this.cancellationReason = '';
  }
  private filterAppointments() {
    let parameters: IAppointmentSearchParametersDto = {
      period: this.searchByDateOption,
      statusNames: this.selectedAppointmentStatuses,
      timeZone: this.timeZoneName,
      inviteeEmail: '',
      eventTypeIds: this.selectedEventTypeIds,
      pageNumber: this.currentPageNumber
    }
    if (Object.keys(this.selectedDateRanges).length == 2) {
      parameters.startDate = this.selectedDateRanges[Object.keys(this.selectedDateRanges)[0]].date;
      parameters.endDate = this.selectedDateRanges[Object.keys(this.selectedDateRanges)[1]].date;
    }
    this.addQueryParamsToUrl(parameters);
    let queryParams = CommonFunction.convertToUriEncodedString(parameters);
    this.appointmentService.getScheduleEvents(queryParams)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.appointmentsPaginationResult = response;
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {

        }
      })
  }
  get statusListControls() {
    return (this.statusListFilterForm.get('statusList') as FormArray).controls;
  }
  get entityTypeListControls() {
    return (this.entityTypesFilterForm.get('entityTypesList') as FormArray).controls;
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}


