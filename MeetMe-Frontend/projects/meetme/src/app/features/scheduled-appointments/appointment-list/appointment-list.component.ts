import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService, IAppointmentDetailsDto, ICancelAppointmentCommand } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.scss']
})
export class AppointmentListComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();

  appointments: IAppointmentDetailsDto[] = [];
  cancellationReason: string = '';

  constructor(private appointmentService: AppointmentService
  ) { }

  ngOnInit(): void {
    this.resetData();
    this.loadAppointments();
  }

  toggleDetails(index: number): void {
    this.appointments[index].isExpanded = !this.appointments[index].isExpanded;
  }
  resetData() {
    this.appointments = [];
    this.cancellationReason = '';
  }

  loadAppointments() {
    this.appointmentService.getAppointments()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.appointments = response;
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
