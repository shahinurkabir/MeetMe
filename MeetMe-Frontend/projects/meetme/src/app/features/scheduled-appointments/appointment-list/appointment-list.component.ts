import { Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AppointmentService, CalendarComponent, CommonFunction, EventTypeService, IAppointmentDetailsDto, IAppointmentSearchParametersDto, IAppointmentsPaginationResult, ICancelAppointmentCommand, IDay, IEventType, MultiCalendarComponent, settings_appointment_search_by_date_option, settings_appointment_status } from '../../../app-core';
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

  constructor(
    private appointmentService: AppointmentService,
    private eventTypeService: EventTypeService,
    private renderer: Renderer2,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
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

    });
  }

  ngOnInit(): void {
    this.configureStatusFilterForm();
    this.resetData();
    this.loadData();
  }

  onToggleAppointmentDetails(item: IAppointmentDetailsDto): void {
    item.isExpanded = !item.isExpanded;
  }
  
  onEventTypesLoaded(eventTypes: IEventType[]) {
    this.eventTypesList = eventTypes;
    this.configureEntityTypesFilterForm();
  }

  onFilterDateOptionClick(filterDateOption: string) {

    if (this.searchByDateOption == filterDateOption) return;

    this.searchByDateOption = filterDateOption;
    this.currentPageNumber = 1;
    this.filterAppointments();

  }

  onFilterByDateRangeClick() {
    this.showDateSelectionWidget = !this.showDateSelectionWidget;

    if (!this.showDateSelectionWidget) return;
    this.searchByDateOption = settings_appointment_search_by_date_option.daterange;
    //reset date range
    if (this.dateRange1.getMonth() + 1 > 12) {
      this.dateRange2 = new Date(this.dateRange1.getFullYear() + 1, 1, 1);
    } else {
      this.dateRange2 = new Date(this.dateRange1.getFullYear(), this.dateRange1.getMonth() + 1, 1);
    }
    this.multiCalendarComponent?.initCalendar(this.dateRange1, this.dateRange2,this.selectedDateRanges);

  }

  onCancelAppointment(appointment: IAppointmentDetailsDto) {
    let cancelAppointmentCommand: ICancelAppointmentCommand = {
      id: appointment.id,
      cancellationReason: appointment.cancellationReason
    }

    this.appointmentService.cancelAppointment(cancelAppointmentCommand)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
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
  }

  onToggleSearchByDateFilterWindow() {
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
  onApplyDateRangeFilter() {
    this.showDateSelectionWidget = false;
    this.currentPageNumber = 1;
    this.selectedDateRanges = this.multiCalendarComponent?.selectedDates || {};
    this.filterAppointments();
  }

  onDateRangeChanged(selecteDates: { [id: string]: IDay }) {
    this.selectedDateRanges = selecteDates;
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
    if (this.isValidDateRange) {
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
  get isValidDateRange() {
    const keys = Object.keys(this.selectedDateRanges);
    return keys.length == 2;
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}


