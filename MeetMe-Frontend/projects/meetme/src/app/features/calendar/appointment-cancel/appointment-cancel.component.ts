import { Component, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { AccountService, AppointmentService, EventTypeService, IAppointmentDetailsDto } from '../../../app-core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-appointment-cancel',
  templateUrl: './appointment-cancel.component.html',
  styleUrls: ['./appointment-cancel.component.scss']
})
export class AppointmentCancelComponent implements OnInit {
  evnetTypeSlugName: string = "";
  eventTypeOwner: string = "";
  appointmentId: string = "";
  appointmentDetails: IAppointmentDetailsDto | undefined
  destroyed$: Subject<boolean> = new Subject<boolean>();
  constructor(
    private route: ActivatedRoute,
    private calendarService: AppointmentService,

  ) { }

  ngOnInit(): void {
    this.appointmentId = this.route.snapshot.paramMap.get("id") ?? "";

    this.loadAppointmentDetails()
  }
  loadAppointmentDetails() {
    this.calendarService.getAppointmentById(this.appointmentId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.appointmentDetails = response;
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
