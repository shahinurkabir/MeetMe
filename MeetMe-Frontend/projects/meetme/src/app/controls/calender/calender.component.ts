import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-calender',
  templateUrl: './calender.component.html',
  styleUrls: ['./calender.component.scss']
})
export class CalenderComponent implements OnInit {
  @Output() dateClicked = new EventEmitter()
  @Input() selectedDates: { [id: string]: string } = {};
  calenderWidget: string = "";
  currentDate: Date = new Date();
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  day_of_week: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
  month_of_year: Array<string> = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "Jun",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
  ];

  days_slot_in_month: { [id: string]: number[] } = {};
  

  constructor() { }

  ngOnInit(): void {
    this.currentDate = new Date();
    this.selectedYear = this.currentDate.getFullYear();
    this.selectedMonth = this.currentDate.getMonth();
    this.intCalendar(this.selectedYear, this.selectedMonth);
    this.selectedDates={};
  }


  onNextMonth() {
    if (this.selectedMonth + 1 > 11) {
      this.selectedMonth = 0;
      this.selectedYear++
    }
    else {
      this.selectedMonth++;
    }
    this.intCalendar(this.selectedYear, this.selectedMonth);
  }
  onPreviousMonth() {
    if (this.selectedMonth - 1 < 0) {
      this.selectedMonth = 11;
      this.selectedYear--
    }
    else {
      this.selectedMonth--;
    }

    this.intCalendar(this.selectedYear, this.selectedMonth);
  }


  intCalendar(yearNo: number, monthNo: number) {


    let html: string;
    let currentDate = new Date(yearNo, monthNo, 1);
    let today = currentDate.getDate();
    let month = currentDate.getMonth();
    let year = currentDate.getFullYear();
    let weekDay = currentDate.getDay();

    this.selectedYearMonth = this.month_of_year[this.selectedMonth] + " " + this.selectedYear;
    this.days_slot_in_month = {};

    html = "<table><thead><tr>"
    this.day_of_week.forEach(dayName => {
      html += `<th>${dayName}</th>`
    })

    html += "</thead></tr>"

    currentDate.setDate(1);
    for (let i = 0; i < weekDay; i++) {
      html += "<td></td>"
    }
    let slot = 0;
    for (let i = 0; i < 31; i++) {
      if (currentDate.getDate() < i) break;
      if (!this.days_slot_in_month[slot])
        this.days_slot_in_month[slot] = [0, 0, 0, 0, 0, 0, 0];
      let weekDay = currentDate.getDay();
      if (weekDay == 0)
        html += "</tr><tr>"
      this.days_slot_in_month[slot][weekDay] = (i + 1);
      html += "<td class='aaa' (click)='function() {alert(i);}'>" + (i + 1) + "</td>"
      if (weekDay == 6) {
        html += "</tr>"
        slot += 1
      }
      currentDate.setDate(currentDate.getDate() + 1)
    }
    this.calenderWidget = html;
  }

  onClickDay(dayNo: number) {

    let shortMonth = this.month_of_year[this.selectedMonth].substring(0, 3);

    let date = dayNo+ "-"+shortMonth +"-"+ this.selectedYear ;

    // toggle date
    if (this.selectedDates[date])
      delete this.selectedDates[date]
    else
      this.selectedDates[date] = date;

    this.dateClicked.emit(this.selectedDates);
  }
}
