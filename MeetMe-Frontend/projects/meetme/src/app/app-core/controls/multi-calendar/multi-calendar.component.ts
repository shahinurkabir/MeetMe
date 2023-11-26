import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IDay } from '../../interfaces';
import { settings_month_of_year } from '../../utilities';

@Component({
  selector: 'app-multi-calendar',
  templateUrl: './multi-calendar.component.html',
  styleUrls: ['./multi-calendar.component.scss']
})
export class MultiCalendarComponent implements OnInit {
  @Output() SelectedDatesChanged = new EventEmitter()

  selectedDates: { [id: string]: IDay } = {};
  calendar1MonthName: string = "";
  calendar2MonthName: string = "";

  calendar1: IDay[][] = [];
  calendar1Month: number = new Date().getMonth() + 1;
  calendar1Year: number = new Date().getFullYear();

  calendar2: IDay[][] = [];
  calendar2Month: number = new Date().getMonth() + 1;
  calendar2Year: number = new Date().getFullYear();


  days: string[] = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  months: string[] = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];
  years: number[] = [];

  constructor() {
  }
  ngOnInit(): void {
  }

  initCalendar(date1: Date, date2: Date,selecteDates:{ [id: string]: IDay } ) {
    this.calendar1Month = date1.getMonth() + 1;
    this.calendar1Year = date1.getFullYear();

    this.calendar2Month = date2.getMonth() + 1;
    this.calendar2Year = date2.getFullYear();
    this.selectedDates = selecteDates;
    this.updateCalendar();

  }

  onClickPreviousMonth() {
    if (this.calendar1Month - 1 < 1) {
      this.calendar1Month = 12;
      this.calendar1Year--;
    } else {
      this.calendar1Month--;
    }
    if (this.calendar2Month - 1 < 1) {
      this.calendar2Month = 12;
      this.calendar2Year--;
    } else {
      this.calendar2Month--;
    }
    this.updateCalendar();
  }
  onClickNextMonth() {
    if (this.calendar1Month + 1 > 12) {
      this.calendar1Month = 1;
      this.calendar1Year++;
    } else {
      this.calendar1Month++;
    }
    if (this.calendar2Month + 1 > 12) {
      this.calendar2Month = 1;
      this.calendar2Year++;
    } else {
      this.calendar2Month++;
    }
    this.updateCalendar();
  }


  updateCalendar() {
    this.calendar1MonthName = `${this.months[this.calendar1Month - 1]} ${this.calendar1Year}`;
    this.calendar2MonthName = `${this.months[this.calendar2Month - 1]} ${this.calendar2Year}`;
    this.updateCalendar1();
    this.updateCalendar2();
    this.updateSelecteDateCalendar();
  }
  updateCalendar1() {
    this.calendar1 = this.generateCalendar(this.calendar1Year, this.calendar1Month);
  }

  updateCalendar2() {
    this.calendar2 = this.generateCalendar(this.calendar2Year, this.calendar2Month);
  }

  generateCalendar(year: number, month: number): IDay[][] {
    const firstDay = new Date(year, month - 1, 1).getDay();
    const daysInMonth = new Date(year, month, 0).getDate();
    const daysInMonthPrevious = new Date(year, month - 1, 0).getDate();
    const previousMonthFirstDate = new Date(year, month - 1 - 1, 1);
    const previousMonth = previousMonthFirstDate.getMonth() + 1;
    const previousMonthYear = previousMonthFirstDate.getFullYear();
    const nextMonthFirstDate = new Date(year, month, 1);
    const nextMonth = nextMonthFirstDate.getMonth() + 1;
    const nextMonthYear = nextMonthFirstDate.getFullYear();
    let currentDate = 1;
    let nextMonthDate = 1;
    const calendar: IDay[][] = [];

    for (let i = 0; i < 6; i++) {
      const week: IDay[] = [];
      for (let j = 0; j < 7; j++) {
        if (i === 0 && j < firstDay) {
          let previousMonthDateString = this.getDateString(previousMonthYear, previousMonth, daysInMonthPrevious - (firstDay - (j + 1)));
          week.push({ dayNo: daysInMonthPrevious - (firstDay - (j + 1)), date: previousMonthDateString, isDifferentMonth: true, isDisabled: false, isSelected: false, isCurrentDate: false }); // Placeholder for days before the 1st of the month
        } else if (currentDate <= daysInMonth) {
          let shortDateString = this.getDateString(year, month, currentDate);
          week.push({ dayNo: currentDate, date: shortDateString, isDifferentMonth: false, isDisabled: false, isSelected: false, isCurrentDate: false });
          currentDate++;
        } else {
          let nextMonthDateString = this.getDateString(nextMonthYear, nextMonth, nextMonthDate);
          week.push({ dayNo: nextMonthDate, date: nextMonthDateString, isDifferentMonth: true, isDisabled: false, isSelected: false, isCurrentDate: false }); // Placeholder for days after the end of the month
          nextMonthDate++;
        }
      }
      calendar.push(week);
    }

    return calendar;
  }

  selectDate(calendar: string, day: IDay) {

    this.selectedDates[day.date] = day;

    const keys = Object.keys(this.selectedDates);
    if (keys.length > 2) {
      this.selectedDates[keys[0]].isSelected = false;
      this.selectedDates[keys[1]].isSelected = false;
      delete this.selectedDates[keys[0]];
      delete this.selectedDates[keys[1]];
    }
    else if (keys.length == 2) {
      let date1 = new Date(keys[0]);
      let date2 = new Date(keys[1]);
      if (date1 > date2) {
        this.selectedDates[keys[0]].isSelected = false;
        delete this.selectedDates[keys[0]];
      }

    }
    this.updateSelecteDateCalendar();
    this.SelectedDatesChanged.emit(this.selectedDates);

  }

  updateSelecteDateCalendar() {
    this.calendar1.forEach((week) => {
      week.forEach((weekDay) => {
        if (this.selectedDates[weekDay.date]) {
          weekDay.isSelected = true;
        } else {
          weekDay.isSelected = false;
        }
      });
    });

    this.calendar2.forEach((week) => {
      week.forEach((weekDay) => {
        if (this.selectedDates[weekDay.date]) {
          weekDay.isSelected = true;
        } else {
          weekDay.isSelected = false;
        }
      });
    });
  }

  private getDateString(year: number, month: number, dayNo: number): string {
    let shortMonth = settings_month_of_year[month - 1].substring(0, 3);
    let dayNoString = dayNo.toString().length == 1 ? "0" + dayNo.toString() : dayNo.toString();
    let date = `${year}-${shortMonth}-${dayNoString}`;
    return date;
  }

}
