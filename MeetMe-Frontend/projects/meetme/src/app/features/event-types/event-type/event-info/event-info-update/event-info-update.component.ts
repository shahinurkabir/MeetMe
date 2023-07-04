import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { IEventType, EventTypeService } from 'projects/meetme/src/app/app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-event-info-update',
  templateUrl: './event-info-update.component.html',
  styleUrls: ['./event-info-update.component.scss']
})
export class EventInfoUpdateComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  model: IEventType = {
    id: "",
    name: "",
    description: "",
    duration: 0,
    eventColor: "",
    ownerId: '',
    activeYN: true,
    location: '',
    slug: '',
    availabilityId: '',
    forwardDuration: 0,
    dateForwardKind: 'moving',
    bufferTimeAfter: 0,
    bufferTimeBefore: 0,
    timeZone: "",
  };
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private location: Location
  ) {

    this.route.parent?.params
      .pipe(takeUntil(this.destroyed$))
      .subscribe((params) => {
        let eventTypeId = params["id"];
        this.loadEventTypeDetail(eventTypeId);

      });

  }

  ngOnInit(): void {
  }

  loadEventTypeDetail(id: string) {
    this.eventTypeService.getById(id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.model = response;
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })

  }
  onSaved(response: any) {
    console.log(`Data saved ${response}`);
    this.location.back();
  }
  onCancelled() {
    this.location.back();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
