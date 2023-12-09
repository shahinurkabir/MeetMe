import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { TimeZoneData } from '../../interfaces/event-type-interfaces';
import { interval } from 'rxjs';
import {  ListOfTimeZone } from '../../utilities/timezone-data';

@Component({
  selector: 'app-timezone-control',
  templateUrl: './timezone-control.component.html',
  styleUrls: ['./timezone-control.component.scss']
})
export class TimezoneControlComponent implements OnInit,OnDestroy {
  @Output() dataLoaded = new EventEmitter()
  @Output() selectionChanged = new EventEmitter<TimeZoneData>();
  @Output() hourFormatChanged = new EventEmitter<boolean>();
  @Input() selectedTimeZone: TimeZoneData | undefined;
  @Input()showHourFormatChangeOption: boolean = false;
  @ViewChild('timezoneContainer') timezoneContainer: ElementRef | undefined;
  @ViewChild('searchTimeZone') searchTimeZone: ElementRef|undefined;
  timeZoneList: TimeZoneData[] = [];
  filterTimeZoneList: TimeZoneData[] = [];
  timeZoneNameFilterText: string = "";
  is24HourFormat: boolean = false;
  
  interval: any;
  timeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;
  constructor() {
    
  }

  ngOnInit(): void {
    this.loadTimeZoneList();
    this.interval = interval(1000).subscribe(val => this.updateTimeZoneLocalTime());
  }
  onClickOutside() {
    this.timezoneContainer?.nativeElement.classList.remove('active');
  }
 
  loadTimeZoneList() {
  
    this.timeZoneList=ListOfTimeZone;
    this.filterTimeZoneList=ListOfTimeZone;

    this.selectedTimeZone = this.timeZoneList.find(e => e.name == this.timeZoneName);

    this.dataLoaded.emit("TimeZoneListLoaded");
  }

  onSelectTimeZone(timeZoneItem: TimeZoneData) {
    this.selectedTimeZone = timeZoneItem;
    this.timeZoneNameFilterText = "";
    this.filterTimeZoneList = this.timeZoneList;
    this.onToggleTimeZoneBox();
    this.selectionChanged.emit(timeZoneItem);
  }

  onToggleTimeZoneBox() {
    this.timezoneContainer?.nativeElement.classList.toggle('active');
    this.searchTimeZone?.nativeElement.focus();
  }
  
  onFilterTimeZoneChanged(event: any) {
    if (this.timeZoneNameFilterText.trim() !== '')
      this.filterTimeZoneList = this.timeZoneList
        .filter(e => e.name.toLowerCase()
          .indexOf(this.timeZoneNameFilterText.toLowerCase()) > -1)
    else
      this.filterTimeZoneList = this.timeZoneList;

  }

  setTimeZone(timeZoneName: string) {
    this.selectedTimeZone = this.timeZoneList.find(e => e.name == timeZoneName);
    this.selectionChanged.emit(this.selectedTimeZone);
  }

  updateTimeZoneLocalTime() {
    this.timeZoneList.forEach(e => {
      if (!this.is24HourFormat)
        e.currentTime = new Date().toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', timeZone: e.name });
      else
        e.currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit', timeZone: e.name });
    });
  }

  onChangeHourFormat(e:any) {
    this.is24HourFormat = e.target.checked;
    this.updateTimeZoneLocalTime();
    this.hourFormatChanged.emit(this.is24HourFormat);
  }

  ngOnDestroy(): void {
    this.interval.unsubscribe();
  }

  
}
