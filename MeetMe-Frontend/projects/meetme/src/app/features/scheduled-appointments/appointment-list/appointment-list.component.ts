import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { AppointmentService, CommonFunction, EventTypeService, IAppointmentDetailsDto, IAppointmentSearchParametersDto, IAppointmentsPaginationResult, ICancelAppointmentCommand, IEventType, settings_appointment_search_by_date_option, settings_appointment_status } from '../../../app-core';
import { Subject, forkJoin, takeUntil } from 'rxjs';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent implements OnInit, OnDestroy {

  @ViewChild('toggleButtonEventTypesFilterWindow') toggleButtonEventTypesFilterWindow: ElementRef | undefined
  @ViewChild('eventTypesFilterWindow') eventTypesFilterWindow: ElementRef | undefined

  @ViewChild('toggleButtonAppointmentStatusByFilterWindow') toggleButtonAppointmentStatusByFilterWindow: ElementRef | undefined
  @ViewChild('appointmentStatusByFilterWindow') appointmentStatusByFilterWindow: ElementRef | undefined


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

  constructor(
    private appointmentService: AppointmentService,
    private eventTypeService: EventTypeService,
    private renderer: Renderer2,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {

    this.renderer.listen('window', 'click', (e: Event) => {

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

  loadData() {
    const appointSearchParameters: IAppointmentSearchParametersDto = {
      status: '',
      searchByDateOption: this.searchByDateOption,
      timeZone: this.timeZoneName,
      inviteeEmail: '',
      eventTypeIds: [],
      filterBy: '',
    }
    forkJoin([
      this.eventTypeService.getList(),
      this.appointmentService.getScheduleEvents(this.currentPageNumber, appointSearchParameters)
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
  get statusListControls() {
    return (this.statusListFilterForm.get('statusList') as FormArray).controls;
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
  get entityTypeListControls() {
    return (this.entityTypesFilterForm.get('entityTypesList') as FormArray).controls;
  }

  toggleDetails(item: IAppointmentDetailsDto): void {
    item.isExpanded = !item.isExpanded;
  }
  resetData() {
    this.cancellationReason = '';
  }

  onEventTypesLoaded(eventTypes: IEventType[]) {
    this.eventTypesList = eventTypes;
    this.configureEntityTypesFilterForm();
  }


  filterAppointments() {
    let pageNumber = 1;
    let parameters: IAppointmentSearchParametersDto = {
      status: '',
      searchByDateOption: this.searchByDateOption,
      timeZone: this.timeZoneName,
      inviteeEmail: '',
      eventTypeIds: this.selectedEventTypeIds,
      filterBy: '',
    }
    this.appointmentService.getScheduleEvents(pageNumber, parameters)
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



  cancelAppointment(appointment: IAppointmentDetailsDto) {
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
  rescheduleAppointment(id: string) {
  }

  toggleEventTypesFilterWindow(e: any) {
    e.preventDefault();
    this.filterByEventTypeYN = !this.filterByEventTypeYN;
    if (this.filterByEventTypeYN) {
      this.configureEntityTypesFilterForm();
    }
  }

  onApplyFilter() {
    this.filterByEventTypeYN = false;
    this.selectedEventTypeIds = [];
    this.filterByEventTypeText = 'All Event Types';

    this.entityTypesFilterForm.value.entityTypesList.forEach((item: any) => {
      if (item.isSelected) {
        this.selectedEventTypeIds.push(item.entityValue);
      }
    });

    let selectedEventTypeIdsCount = this.selectedEventTypeIds.length;

    if (selectedEventTypeIdsCount == 1)
      this.filterByEventTypeText = "1 Event Type";
    else
      this.filterByEventTypeText = selectedEventTypeIdsCount + " Event Types";

    this.filterAppointments();

  }
  onCancelFilter() {
    this.filterByEventTypeYN = false;
  }

  updateFilterFieldsText() {
    this.filterByEventTypeText = 'All Event Types';
    this.filterByStatusText = 'Active Events';
    this.filterByInviteeEmailText = 'All Invitee Emails';

    let selectedEventTypeIdsCount = this.selectedEventTypeIds.length;
    let selectedInviteeEmailsCount = this.selectedInviteeEmails.length;

    if (selectedEventTypeIdsCount == 1)
      this.filterByEventTypeText = "1 Event Type";
    else
      this.filterByEventTypeText = selectedEventTypeIdsCount + " Event Types";

    if (selectedInviteeEmailsCount == 1)
      this.filterByInviteeEmailText = "1 Invitee Email";
    else
      this.filterByInviteeEmailText = selectedInviteeEmailsCount + " Invitee Emails";
  }



  toggleAppointmentStatusFilterWindow(e: any) {
    e.preventDefault();
    this.filterByAppointmentStatusYN = !this.filterByAppointmentStatusYN;
    if (this.filterByAppointmentStatusYN) {
      this.configureStatusFilterForm();
    }
  }

  onApplyStatusFilter() {
    this.filterByAppointmentStatusYN = false;
    this.selectedAppointmentStatuses = [];
    this.statusListFilterForm.value.statusList.forEach((item: any) => {
      if (item.isSelected) {
        this.selectedAppointmentStatuses.push(item.statusValue);
      }
    });

    this.filterAppointments();

    this.filterByStatusText = this.selectedAppointmentStatuses.length == 1 ? CommonFunction.capitalizeFirstLetter(this.selectedAppointmentStatuses[0]) + " Event" : 'All Events';
  }
  onCancelStatusFilter() {
    this.filterByAppointmentStatusYN = false;
  }

  toggleInviteeEmailFilterWindow() {
  }
  toggleSearchByDateFilterWindow() {
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}


