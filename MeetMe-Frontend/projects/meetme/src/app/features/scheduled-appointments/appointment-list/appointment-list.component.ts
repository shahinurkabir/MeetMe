import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppointmentService,  IAppointmentDetailsDto, ICancelAppointmentCommand } from '../../../app-core';
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
  cancelAppointment (id: string) {
    let cancelAppointmentCommand:ICancelAppointmentCommand = {
      id: id,
      cancellationReason: this.cancellationReason
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

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
