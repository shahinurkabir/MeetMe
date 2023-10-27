import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IEventType, EventTypeService, AuthService, ModalService, AlertService } from '../../../app-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-eventtype',
  templateUrl: './event-type-list.component.html',
  styleUrls: ['./event-type-list.component.css']
})
export class EventTypeListComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  listEventTypes: IEventType[] = [];
  itemToDelete: IEventType | undefined;
  baseUri: string = "";
  user_name: string = "";
  host: string = window.location.host;
  constructor(
    private eventTypeService: EventTypeService,
    private router: Router,
    private modalService: ModalService,
    private authService: AuthService,
    private alertService: AlertService
  ) {
    this.modalService.reset();
    this.baseUri = this.authService.baseUri;
    this.user_name = this.authService.userName;
  }


  ngOnInit(): void {
    this.loadEventTypes();
  }

  loadEventTypes() {
    this.eventTypeService.getList()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.listEventTypes = response
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })
  }

  getDynamicStyle(item: any): object {
    return {
      border: `1px solid lightgray`,
      'border-left': `5px solid ${item.eventColor}`
    }
  }
  onEdit(id: string) {
    let url = '/' + id;
    this.router.navigate(['event-types', id]);
  }
  onAddNew() {
    this.router.navigate(['event-types', 'new']);
  }

  onToggleActive(eventType: IEventType) {
    this.eventTypeService.toggleStatus(eventType.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          eventType.activeYN = !eventType.activeYN;
          this.alertService.success(`Event Type ${eventType.activeYN?"Activated":"Deactivated"} successfully`);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
  }
  onClone(eventType: IEventType) {
    this.eventTypeService.clone(eventType.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Event Type cloned successfully");
          let url = 'event-types/' + response;
          this.router.navigate([url]);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })
  }
  onDelete(eventType: IEventType) {
    this.eventTypeService.delete(this.itemToDelete?.id!)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Event Type deleted successfully");
          this.loadEventTypes();
        },
        error: (error) => { console.log(error) },
        complete: () => { this.modalService.close() }
      })
  }
  onClickDelete(itemToDelete: IEventType) {
    this.itemToDelete = itemToDelete;
    this.modalService.open('delete-eventtype-modal')
  }

  onCloseModal() {
    this.modalService.close();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
