import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { day_of_week, month_of_year } from '../../constants/default-data';

@Component({
  selector: 'app-calender',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  @Output() dateClicked = new EventEmitter()
  @Input() selectedDates: { [id: string]: string } = {};
  calenderWidget: string = "";
  currentDate: Date = new Date();
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  weekDays = day_of_week;
  days_slot_in_month: { [weekNo: string]: IDay[] } = {};


  constructor() { }

  ngOnInit(): void {
    this.currentDate = new Date();
    this.selectedYear = this.currentDate.getFullYear();
    this.selectedMonth = this.currentDate.getMonth();
    this.updateCalendar(this.selectedYear, this.selectedMonth);
    this.selectedDates = {};
  }


  onNextMonth() {
    if (this.selectedMonth + 1 > 11) {
      this.selectedMonth = 0;
      this.selectedYear++
    }
    else {
      this.selectedMonth++;
    }
    this.updateCalendar(this.selectedYear, this.selectedMonth);
  }

  onPreviousMonth() {
    if (this.selectedMonth - 1 < 0) {
      this.selectedMonth = 11;
      this.selectedYear--
    }
    else {
      this.selectedMonth--;
    }

    this.updateCalendar(this.selectedYear, this.selectedMonth);

  }

  updateCalendar(yearNo: number, monthNo: number) {
    let html: string;
    let currentDate = new Date(yearNo, monthNo, 1);
    let weekDay = currentDate.getDay();

    this.selectedYearMonth = month_of_year[this.selectedMonth] + " " + this.selectedYear;
    this.days_slot_in_month = {};

    html = "<table><thead><tr>"
    this.weekDays.forEach(dayName => {
      html += `<th>${dayName}</th>`
    })

    html += "</thead></tr>"
    for(let i=0;i<7;i++){
      let weekDays:IDay[]=[];
      for (let j=0;j<6;j++) {
        weekDays.push({ dayNo:0,isSelected:false})
      }
      this.days_slot_in_month[i]=weekDays;
    }

    currentDate.setDate(1);
    for (let i = 0; i < weekDay; i++) {
      html += "<td></td>"
    }
    let slot = 0;
    for (let i = 0; i < 31; i++) {
      if (currentDate.getDate() < i) break;
      //if (!this.days_slot_in_month[slot])
      //  this.days_slot_in_month[slot] = [0, 0, 0, 0, 0, 0, 0];
      let weekDay = currentDate.getDay();
      this.days_slot_in_month[slot][weekDay] = {dayNo: (i + 1),isSelected:false};
      if (weekDay == 6) {
        slot += 1
      }
      currentDate.setDate(currentDate.getDate() + 1)
    }
  }

  onClickDay(weekDay: IDay) {

    weekDay.isSelected=!weekDay.isSelected;

    let shortMonth = month_of_year[this.selectedMonth].substring(0, 3);

    let date =`${this.selectedYear}-${shortMonth}-${weekDay.dayNo}` ;

    // toggle date
    if (this.selectedDates[date])
      delete this.selectedDates[date]
    else
      this.selectedDates[date] = date;

    this.dateClicked.emit(this.selectedDates);
  }

  resetSelection(selectDay?:number) {
    this.selectedDates={};
    let selectedDay:IDay|undefined;
    for (let week in this.days_slot_in_month){
      let listDaysInWeek=this.days_slot_in_month[week];
      listDaysInWeek.forEach(day=>day.isSelected=(day.dayNo==selectDay!));
    }
    if (selectDay) this.onClickDay({dayNo:selectDay!,isSelected:false})
  }
}
interface IDay {
  dayNo:number,
  isSelected:boolean
}