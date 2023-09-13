import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService, AppointmentService, EventTypeService, IAccountProfileInfo, IAppointmentDetailsDto, IEventType } from '../../../app-core';
import { Subject, forkJoin, takeUntil } from 'rxjs';

@Component({
  selector: 'app-appointment-detail',
  templateUrl: './appointment-detail.component.html',
  styleUrls: ['./appointment-detail.component.scss']
})
export class AppointmentDetailComponent implements OnInit, OnDestroy {
  evnetTypeSlugName: string = "";
  eventTypeOwner: string = "";
  appointmentId: string = "";
  eventTypeInfo: IEventType | undefined;
  eventTypeOwnerInfo: IAccountProfileInfo | undefined;
  appointmentDetails: IAppointmentDetailsDto | undefined
  destroyed$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private calendarService: AppointmentService,
    private accountService: AccountService,
    private eventTypeService: EventTypeService

  ) { }

  ngOnInit(): void {
    this.evnetTypeSlugName = this.route.snapshot.paramMap.get("slug") ?? "";
    this.eventTypeOwner = this.route.snapshot.paramMap.get("user") ?? "";
    this.appointmentId = this.route.snapshot.paramMap.get("id") ?? "";

    this.loadAllData()
  }

  loadAllData() {
    forkJoin([
      this.calendarService.getAppointmentById(this.appointmentId),
      this.accountService.getProfileByUserName(this.eventTypeOwner),
      this.eventTypeService.getBySlugName(this.evnetTypeSlugName)
    ])
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.appointmentDetails = response[0];
          this.eventTypeOwnerInfo = response[1];
          this.eventTypeInfo = response[2];
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
