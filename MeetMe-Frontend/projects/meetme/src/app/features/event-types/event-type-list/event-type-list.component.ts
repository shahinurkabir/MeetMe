import { Component, ElementRef, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IEventType, EventTypeService, AuthService, AlertService, ClipboardService } from '../../../app-core';
import { Subject, forkJoin, takeUntil } from 'rxjs';

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
  selectedEventTypesCount: number = 0;
  constructor(
    private el: ElementRef,
    private eventTypeService: EventTypeService,
    private router: Router,
    private authService: AuthService,
    private alertService: AlertService,
    private clipboardService: ClipboardService
  ) {
    this.baseUri = this.authService.baseUri;
    this.user_name = this.authService.userName;
  }


  ngOnInit(): void {
    this.loadEventTypes();
  }
  onDropdownClick(event: Event) {
    event.stopPropagation();
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event) {
    const clickedInside = this.el.nativeElement.contains(event.target);

    if (!clickedInside) {
      this.showHideDropdownMenus('');
    }
    else {
      const element = event.target as HTMLElement;
      const id = element.id;
      this.showHideDropdownMenus(id);
    }
  }

  showHideDropdownMenus(id?: string) {
    const elements = this.el.nativeElement.querySelectorAll('.btn-overlay-opener') as NodeListOf<HTMLElement>;
    elements.forEach(element => {
      if (element.id == id) {
        element.classList.toggle('show');
      }
      else
        element.classList.remove('show');
    })
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
  getColorIndicatorStyle(item: any): object {
    return {
      'background-color': `${item.eventColor}`
    }
  }
  onEdit(id: string) {
    let url = '/' + id;
    this.router.navigate(['event-types', id]);
  }
  onAddNew() {
    this.router.navigate(['event-types', 'new']);
  }
  onClickCopyLink(copyLinkEl: HTMLElement, eventSlug: string) {
    let url = `${this.host}/calendar/${this.baseUri}/${eventSlug}`
    this.clipboardService.copyToClipboard(url);
    copyLinkEl.classList.add('copy-link-done');
    setTimeout(() => {
      copyLinkEl.classList.remove('copy-link-done');
    }, 1000);
  }
  onToggleStatus(eventType: IEventType) {
    this.eventTypeService.toggleStatus(eventType.id)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.showHideDropdownMenus();
          eventType.activeYN = !eventType.activeYN;
          this.alertService.success(`Event Type ${eventType.activeYN ? "Activated" : "Deactivated"} successfully`);
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
          this.showHideDropdownMenus();
          this.alertService.success("Event Type cloned successfully");
          let url = 'event-types/' + response;
          this.router.navigate([url]);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })
  }
  onDelete() {
    this.eventTypeService.delete(this.itemToDelete?.id!)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.showHideDropdownMenus();
          this.itemToDelete = undefined;
          this.alertService.success("Event Type deleted successfully");
          this.loadEventTypes();
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      })
  }
  onEventTypeSelectionChnaged(e: any) {
    this.selectedEventTypesCount = this.listEventTypes.filter(x => x.isSelected).length;
  }
  onDeleteConfirm(itemToDelete: IEventType) {
    this.itemToDelete = itemToDelete;
  }

  onCancelDelete() {
    this.itemToDelete = undefined;
  }
  onToggleStatusSelected() {
    let selectedEventTypes = this.listEventTypes.filter(x => x.isSelected);
    if (selectedEventTypes.length == 0) return;
    let ids = selectedEventTypes.map(x => x.id);

    forkJoin(ids.map(id => this.eventTypeService.toggleStatus(id)))
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.showHideDropdownMenus();
          this.alertService.success("Event Types Toggling status changed successfully");
          this.loadEventTypes();
          this.onClearSelection();
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });


  }
  onDeleteSelected() {
    let selectedEventTypes = this.listEventTypes.filter(x => x.isSelected);
    if (selectedEventTypes.length == 0) return;
    let ids = selectedEventTypes.map(x => x.id);
    forkJoin(ids.map(id => this.eventTypeService.delete(id)))
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Event Types deleted successfully");
          this.showHideDropdownMenus();
          this.loadEventTypes();
          this.onClearSelection();
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
  }
  onClearSelection() {
    this.listEventTypes.forEach(x => x.isSelected = false);
    this.selectedEventTypesCount = 0;
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
