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
  @Input() date1: Date = new Date(new Date().toISOString().split('T')[0]);
  @Input() date2: Date = new Date(new Date().toISOString().split('T')[0]);
  calendar1MonthName: string = "";
  calendar2MonthName: string = "";

  selectedMonth1: number = new Date().getMonth() + 1;
  selectedYear1: number = new Date().getFullYear();
  calendar1: IDay[][] = [];
  selectedDate1: IDay | null = null;

  selectedMonth2: number = new Date().getMonth() + 1;
  selectedYear2: number = new Date().getFullYear();
  calendar2: IDay[][] = [];
  selectedDate2: IDay | null = null;


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
    this.selectedMonth1 = this.date1.getMonth() + 1;
    this.selectedYear1 = this.date1.getFullYear();

    this.selectedMonth2 = this.date2.getMonth() + 1;
    this.selectedYear2 = this.date2.getFullYear();
    this.updateCalendar();
  }

  initCalendar(date1: Date, date2: Date,selecteDates:{ [id: string]: IDay } ) {
    this.selectedMonth1 = date1.getMonth() + 1;
    this.selectedYear1 = date1.getFullYear();

    this.selectedMonth2 = date2.getMonth() + 1;
    this.selectedYear2 = date2.getFullYear();
    this.selectedDates = selecteDates;
    this.updateCalendar();

  }

  onClickPreviousMonth() {
    if (this.selectedMonth1 - 1 < 1) {
      this.selectedMonth1 = 12;
      this.selectedYear1--;
    } else {
      this.selectedMonth1--;
    }
    if (this.selectedMonth2 - 1 < 1) {
      this.selectedMonth2 = 12;
      this.selectedYear2--;
    } else {
      this.selectedMonth2--;
    }
    this.updateCalendar();
  }
  onClickNextMonth() {
    if (this.selectedMonth1 + 1 > 12) {
      this.selectedMonth1 = 1;
      this.selectedYear1++;
    } else {
      this.selectedMonth1++;
    }
    if (this.selectedMonth2 + 1 > 12) {
      this.selectedMonth2 = 1;
      this.selectedYear2++;
    } else {
      this.selectedMonth2++;
    }
    this.updateCalendar();
  }


  updateCalendar() {
    this.calendar1MonthName = `${this.months[this.selectedMonth1 - 1]} ${this.selectedYear1}`;
    this.calendar2MonthName = `${this.months[this.selectedMonth2 - 1]} ${this.selectedYear2}`;
    this.updateCalendar1();
    this.updateCalendar2();
    this.updateSelecteDateCalendar();
  }
  updateCalendar1() {
    this.calendar1 = this.generateCalendar(this.selectedYear1, this.selectedMonth1);
  }

  updateCalendar2() {
    this.calendar2 = this.generateCalendar(this.selectedYear2, this.selectedMonth2);
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
    // day.isSelected = true;

    console.log(day);
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
