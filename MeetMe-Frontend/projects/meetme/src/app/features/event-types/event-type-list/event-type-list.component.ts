import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IEventType } from '../../../interfaces/event-type-interfaces';
import { EventTypeService } from '../../../services/eventtype.service';
import { ModalService } from '../../../controls/modal/modalService';

@Component({
  selector: 'app-eventtype',
  templateUrl: './event-type-list.component.html',
  styleUrls: ['./event-type-list.component.css']
})
export class EventTypeListComponent implements OnInit {

  listEventTypes: IEventType[] = [];
  itemToDelete: IEventType | undefined;
  constructor(private eventTypeService: EventTypeService,
    private router: Router,
    private modalService: ModalService
  ) {
    this.modalService.reset();
  }

  ngOnInit(): void {
    this.loadEventTypes();
  }

  loadEventTypes() {
    this.eventTypeService.getList().subscribe({
      next: response => {
        this.listEventTypes = response
      },
      error: (error) => { console.log(error) },
      complete: () => { }
    })
  }
  onAddNewEventType() {

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
    this.eventTypeService.toggleActive(eventType.id).subscribe(response => {
      eventType.activeYN = !eventType.activeYN;
    })
  }
  onClone(eventType: IEventType) {
    this.eventTypeService.clone(eventType.id).subscribe(response => {
      let url = 'event-types/' + response;
      this.router.navigate([url]);
    })
  }
  onDelete(eventType: IEventType) {
    this.eventTypeService.delete(this.itemToDelete?.id!).subscribe(
      {
        next: response => {
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
}
