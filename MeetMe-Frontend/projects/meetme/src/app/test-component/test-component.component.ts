import { Component, OnInit, ViewChild } from '@angular/core';
import { TimeZoneData } from '../interfaces/event-type-interfaces';
import { TimezoneControlComponent } from '../controls/timezone-control/timezone-control.component';

@Component({
  selector: 'app-test-component',
  templateUrl: './test-component.component.html',
  styleUrls: ['./test-component.component.scss']
})
export class TestComponentComponent implements OnInit {
  @ViewChild("timezoneControl", { static: true }) timezoneControl: TimezoneControlComponent | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  defaultTimeZone: string = "Bangladesh Standard Time";
  constructor() { }

  ngOnInit(): void {
  }
  onLoadedTimezoneData(e:any) {
    console.log(e);
    this.timezoneControl?.setTimeZone(this.defaultTimeZone);
  }
  onChangedTimezone(e:TimeZoneData) {
    console.log(e);
  }
}
