import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EventType } from '../../../models/eventtype';
import { EventTypeService } from '../../../services/eventtype.service';

@Component({
  selector: 'app-eventtype',
  templateUrl: './event-type-list.component.html',
  styleUrls: ['./event-type-list.component.css']
})
export class EventTypeListComponent implements OnInit {

  listEventTypes: EventType[] = [];
  constructor(private eventTypeService: EventTypeService,
    private router:Router
    ) { }

  ngOnInit(): void {
    this.loadEventTypes();
  }

  loadEventTypes() {
    this.eventTypeService.getList().subscribe(response => {
      this.listEventTypes = response
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
  onEdit(id:string) {
    let url='/'+id;
    this.router.navigate(['event-types',id]);
  }
  onAddNew() {
    this.router.navigate(['event-types','new']);
  }

  onToggleActive(eventType: EventType) {
    this.eventTypeService.toggleActive(eventType.id).subscribe(response => {
      eventType.activeYN = !eventType.activeYN;
    })
  }
  onClone(eventType: EventType) {
    this.eventTypeService.clone(eventType.id).subscribe(response => {
      let url='event-types/'+response;
      this.router.navigate([url]);
    })
  }
  onDelete(eventType: EventType) {
    this.eventTypeService.delete(eventType.id).subscribe(response => {
      this.loadEventTypes();
    })
  }
}
