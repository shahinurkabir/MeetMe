import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ITimeIntervalsInDay } from '../event-availability.component';

@Component({
  selector: 'app-daily-time-intervals',
  templateUrl: './daily-time-intervals.component.html',
  styleUrls: ['./daily-time-intervals.component.scss']
})
export class DailyTimeIntervalsComponent implements OnInit {
  @Input() timeIntervalsInDay: ITimeIntervalsInDay | undefined;
  @Output() onFocusChanged = new EventEmitter();
  @Output() onIntervalRemovedEvent = new EventEmitter();
  @Output() onIntervalAddedEvent = new EventEmitter()

  constructor() { }

  ngOnInit(): void {
  }
  onLostFocus(e: Event, index: number, isEndTime: boolean, dailyTimeAvailabilities: ITimeIntervalsInDay) {
    this.onFocusChanged.emit({ e, index, isEndTime, dailyTimeAvailabilities })
  }
  onRemoveTimeInterval(i: number, timeIntervalsInDay: ITimeIntervalsInDay) {
    this.onIntervalRemovedEvent.emit({ i, timeIntervalsInDay })
  }
  onAddTimeInterval(timeIntervalsInDay: ITimeIntervalsInDay) {
    this.onIntervalAddedEvent.emit({ timeIntervalsInDay })
  }
}
