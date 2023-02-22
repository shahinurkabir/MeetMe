import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { day_of_week, month_of_year } from '../../constants/default-data';

@Component({
  selector: 'app-calender',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  @Output() handlerDateClick = new EventEmitter()
  @Input() selectedDates: { [id: string]: string | undefined } = {};
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  weekDays = day_of_week;
  days_in_month: { [weekNo: string]: IDay[] } = {};


  constructor() { }

  ngOnInit(): void {
    let currentDate = new Date();
    this.selectedYear = currentDate.getFullYear();
    this.selectedMonth = currentDate.getMonth();
    this.selectedDates = {};
    this.updateCalendar(this.selectedYear, this.selectedMonth);
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
    let currentDate = new Date(yearNo, monthNo, 1);

    this.selectedYearMonth = month_of_year[this.selectedMonth] + " " + this.selectedYear;
    this.days_in_month = {};

    for (let i = 0; i < 7; i++) {
      let weekDays: IDay[] = [];
      for (let j = 0; j < 6; j++) {
        weekDays.push({ dayNo: 0, isSelected: false, date: "" })
      }
      this.days_in_month[i] = weekDays;
    }

    currentDate.setDate(1);

    let weekNo = 0;
    
    for (let i = 0; i < 31; i++) {
      
      if (currentDate.getDate() < i) break;
      
      let weekDay = currentDate.getDay();
      let dayNo = i + 1
      let shortDateString = this.getShortDateString(dayNo);
      this.days_in_month[weekNo][weekDay] = { dayNo: dayNo, date: shortDateString, isSelected: false };
      
      if (weekDay == 6) {
        weekNo += 1
      }

      currentDate.setDate(currentDate.getDate() + 1)

    }
  }

  onClickDay(weekDay: IDay) {

    weekDay.isSelected = !weekDay.isSelected;

    if (this.selectedDates[weekDay.date])
      this.selectedDates[weekDay.date] = undefined
    else
      this.selectedDates[weekDay.date] = weekDay.date;

    this.handlerDateClick.emit(this.selectedDates);
  }

  resetSelection(defaultSelectedDay?: number) {

    this.selectedDates = {};

    for (let week in this.days_in_month) {
      let listDaysInWeek = this.days_in_month[week];
      listDaysInWeek.forEach(day => day.isSelected = (day.dayNo == defaultSelectedDay!));
    }

    if (defaultSelectedDay) {
      let date = this.getShortDateString(defaultSelectedDay);
      this.selectedDates[date] = date;
      this.handlerDateClick.emit(this.selectedDates);
    }
  }

  private getShortDateString(dayNo: number): string {

    let shortMonth = month_of_year[this.selectedMonth].substring(0, 3);

    let date = `${this.selectedYear}-${shortMonth}-${dayNo}`;

    return date;
  }
}
interface IDay {
  dayNo: number,
  date: string,
  isSelected: boolean
}