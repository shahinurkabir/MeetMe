import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService, IAppointmentDetailsDto, IAppointmentSearchParametersDto, IAppointmentsPaginationResult, ICancelAppointmentCommand } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();

  appointmentsPaginationResult: IAppointmentsPaginationResult | undefined;
  cancellationReason: string = '';
  timeZoneName: string = Intl.DateTimeFormat().resolvedOptions().timeZone;
  constructor(private appointmentService: AppointmentService
  ) {

  }

  ngOnInit(): void {
    this.resetData();
    this.loadAppointments();
  }

  toggleDetails(item: IAppointmentDetailsDto): void {
    item.isExpanded = !item.isExpanded;
  }
  resetData() {
    this.cancellationReason = '';
  }

  loadAppointments() {
    let pageNumber = 1;
    let parameters: IAppointmentSearchParametersDto = {
      status: '',
      searchBy: 'P',
      timeZone: this.timeZoneName,
      inviteeEmail: '',
      eventTypeIds: [],
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
          this.loadAppointments();
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


  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
