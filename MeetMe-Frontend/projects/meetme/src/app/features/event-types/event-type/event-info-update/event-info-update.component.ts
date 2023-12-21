import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { IEventType, EventTypeService, settings_meeting_forward_Duration_inDays, setting_meetting_forward_Duration_kind, CommonFunction } from 'projects/meetme/src/app/app-core';
import { Subject, takeUntil } from 'rxjs';
import { EventInfoModalComponent } from '../event-info-modal.component/event-info-modal.component';

@Component({
  selector: 'app-event-info-update',
  templateUrl: './event-info-update.component.html',
  styleUrls: ['./event-info-update.component.scss']
})
export class EventInfoUpdateComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  @ViewChild('eventInfoComponent', { static: true }) eventInfoComponent!: EventInfoModalComponent;

  eventTypeId: string = "";

  constructor(
    private route: ActivatedRoute,
  ) {

    this.route.parent?.params
      .pipe(takeUntil(this.destroyed$))
      .subscribe((params) => {
        this.eventTypeId = params["id"];
        
      });
      
    }
    
    ngOnInit(): void {
    //this.eventInfoComponent.loadEventTypeDetail(this.eventTypeId);
  }

  onSaved(response: any) {
    console.log(`Data saved ${response}`);
  }
  onCancelled() {
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
