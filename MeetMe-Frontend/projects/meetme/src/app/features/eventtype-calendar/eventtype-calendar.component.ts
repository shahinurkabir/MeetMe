import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CalendarComponent } from '../../controls/calender/calendar.component';
import { CalendarService } from '../../services/calendar.service';
import { IEventTimeAvailability } from '../../models/calendar';
import { TimeZoneData } from '../../models/eventtype';
import { TimeZoneService } from '../../services/timezone.service';
import { TimezoneControlComponent } from '../../controls/timezone-control/timezone-control.component';

@Component({
  selector: 'app-event-type-calendar',
  templateUrl: './eventtype-calendar.component.html',
  styleUrls: ['./eventtype-calendar.component.scss']
})
export class EventTypeCalendarComponent implements OnInit {
  @ViewChild(CalendarComponent) calendarComponent!: CalendarComponent;
  //@ViewChild('timezoneContainer') timezoneContainer: ElementRef | undefined;
  @ViewChild("timezoneControl", { static: true }) timezoneControl: TimezoneControlComponent | undefined;
  days: IEventTimeAvailability[] = [];
  day: IEventTimeAvailability | undefined;
  selectedTimeZone: TimeZoneData | undefined;
  timeZoneNameFilterText: string = "";
  timeZoneList: TimeZoneData[] = [];
  filterTimeZoneList: TimeZoneData[] = [];

  selectedYear:number=0;
  selectedMonth:number=0;
  constructor(private calendarService: CalendarService,private timeZoneService: TimeZoneService,) { }


  ngOnInit(): void {
    this.loadTimeZoneList();
    //this.loadCalendar();
    //this.calendarComponent.resetSelection();
  }

  ngViewAfterInit() {
  }
  
  onHandledCalendarClicked(e:any){
    console.log(e);
    if (Object.keys(e).length==0)  return ;

    let lastDate = Object.keys(e)[Object.keys(e).length-1];
    
    this.day=this.days.find(e=>e.date==lastDate);
  }

  onHandledMonthChange(e:any){
    this.selectedYear=e.year;
    this.selectedMonth=e.month;
  
    this.loadEventTimeAvailability();
  }

  loadEventTimeAvailability() {

    let daysInMonth=new Date(this.selectedYear,this.selectedMonth+1,0).getDate();
    let fromDate=this.selectedYear+"-"+(this.selectedMonth+1)+"-01";
    let toDate=this.selectedYear+"-"+(this.selectedMonth+1)+"-"+daysInMonth;
    let timezone="Bangladesh Standard Time";

    if (this.selectedTimeZone)
      timezone = this.selectedTimeZone.name;

    this.calendarService.getList(timezone,fromDate,toDate).subscribe((data) => {
      this.days = data;
      this.days.forEach((day) => {
        day.slots.forEach((slot) => {
          slot.startAtTimeOnly = new Date(slot.startAt)
            .toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', timeZone: this.selectedTimeZone?.name });
        });
      });
      this.day = this.days[0];

    });
  }

  loadTimeZoneList() {
    this.timeZoneService.getList().subscribe(res => {
      console.log(res)
      this.timeZoneList = res;
      this.filterTimeZoneList = res;
      // if (this.availability?.timeZoneId)
      //   this.selectedTimeZone = this.timeZoneList.find(e => e.id == this.availability?.timeZoneId);
    })
  }

  // onToggleCountryDropdownBox() {
  //   this.timezoneContainer?.nativeElement.classList.toggle('active');
  // }

  // onFilterTimeZoneChanged(event: any) {
  //   if (this.timeZoneNameFilterText.trim() !== '')
  //     this.filterTimeZoneList = this.timeZoneList
  //       .filter(e => e.name.toLowerCase()
  //         .indexOf(this.timeZoneNameFilterText.toLowerCase()) > -1)
  //   else
  //     this.filterTimeZoneList = this.timeZoneList;

  // }
  // onSelectTimeZone(timeZoneItem: TimeZoneData) {
  //   this.selectedTimeZone = timeZoneItem;
  //   this.timeZoneNameFilterText = "";
  //  // this.onToggleCountryDropdownBox();
  //   this.loadEventTimeAvailability();

  // }
  onLoadedTimezoneData(e:any) {
    console.log(e);
    //this.timezoneControl?.setTimeZone(this.defaultTimeZone);
  }
  onChangedTimezone(e:TimeZoneData) {
    console.log(e);
    this.selectedTimeZone = e;
    //this.loadEventTimeAvailability();
    this.days.forEach((day) => {
      day.slots.forEach((slot) => {
        slot.startAtTimeOnly = new Date(slot.startAt)
          .toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', timeZone: this.selectedTimeZone?.name });
      });
    });
  }
}
