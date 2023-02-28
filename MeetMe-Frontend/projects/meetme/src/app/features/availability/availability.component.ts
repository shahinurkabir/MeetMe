import { Component, OnInit, ViewChild } from '@angular/core';
import { TimeAvailabilityComponent } from '../../controls/time-availability/time-availability.component';

@Component({
  selector: 'app-availability',
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss']
})
export class AvailabilityComponent implements OnInit {
  @ViewChild(TimeAvailabilityComponent) timeAvailabilityComponent!: TimeAvailabilityComponent;
  constructor() { }

  ngOnInit(): void {
  }
  onSave(event: any) {
    event.preventDefault();
    let availability=this.timeAvailabilityComponent.getAvailability();

    console.log(availability);

  }
}
