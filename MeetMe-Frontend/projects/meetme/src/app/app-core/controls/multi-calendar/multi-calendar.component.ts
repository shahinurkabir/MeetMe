import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IDay } from '../../interfaces';
import { settings_appointment_search_by_date_option, settings_month_of_year } from '../../utilities';

@Component({
  selector: 'app-multi-calendar',
  templateUrl: './multi-calendar.component.html',
  styleUrls: ['./multi-calendar.component.scss']
})
export class MultiCalendarComponent implements OnInit {
  @Output() DatePeriodChanged = new EventEmitter()
  @Output() DateSelectionChanged = new EventEmitter()
  @Output() CancelDateSelection = new EventEmitter()
  selectedPeriod: string = '';
  selectedDates: { [id: string]: IDay } = {};

  calendar1MonthName: string = "";
  calendar2MonthName: string = "";

  calendar1: IDay[][] = [];
  calendar2: IDay[][] = [];

  calendar1Month: number = 0;
  calendar1Year: number = 0;

  calendar2Month: number = 0;
  calendar2Year: number = 0;


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
    this.resetDates();
  }
  resetDates() {

    let calendar1date = new Date();
    this.calendar1Month = calendar1date.getMonth() + 1;
    this.calendar1Year = calendar1date.getFullYear();

    let calendar2date = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 1);
    this.calendar2Month = calendar2date.getMonth() + 1;
    this.calendar2Year = calendar2date.getFullYear();

    let dates = this.generateDates(this.selectedPeriod);

    this.updateCalendar();

    this.resetSelectedDates();

    let date1 = dates[0];
    let date2 = dates[1];
    let dateShort1 = this.getDateString(date1.getFullYear(), date1.getMonth() + 1, date1.getDate());
    let dateShort2 = this.getDateString(date2.getFullYear(), date2.getMonth() + 1, date2.getDate());

    this.selectedDates[this.getDateKey('calendar1', dateShort1)] = { dayNo: date1.getDate(), date: dateShort1, isDifferentMonth: false, isDisabled: false, isSelected: true, isCurrentDate: this.isCurrentDate(dateShort1) };
    this.selectedDates[this.getDateKey('calendar2', dateShort2)] = { dayNo: date1.getDate(), date: dateShort2, isDifferentMonth: false, isDisabled: false, isSelected: true, isCurrentDate: this.isCurrentDate(dateShort2) };

    this.updateSelectedDates();
    this.updateSelecteDateCalendar();
  }

  private generateDates(intervalName: string): Date[] {
    let currentDate = new Date();
    let date1 = new Date();
    let date2 = new Date();
    switch (intervalName) {
      case settings_appointment_search_by_date_option.daterange:
        let keys = Object.keys(this.selectedDates);
        date1 = new Date(this.selectedDates[keys[0]].date);
        date2 = new Date(this.selectedDates[keys[1]].date);;
        break;
      case 'today':
        date1 = currentDate;
        date2 = currentDate;
        break;
      case 'tomorrow':
        date1.setDate(currentDate.getDate() + 1);
        date2.setDate(currentDate.getDate() + 1);
        break;
      case 'this_week':
        date1.setDate(currentDate.getDate() - currentDate.getDay());
        date2.setDate(currentDate.getDate() + (6 - currentDate.getDay()));
        break;

      case 'this_month':
        date1.setDate(1);
        date2.setDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0).getDate());
        break;
      case 'alltime':
        break;
      default:
        break;
    }
    return [date1, date2];
  }
  onIntervalClick(intervalName: string) {
    this.selectedPeriod = intervalName;

    if (this.selectedPeriod == 'alltime') {
      this.DatePeriodChanged.emit(this.selectedPeriod);
      return;
    }

    let dates = this.generateDates(intervalName);

    let date1 = dates[0];
    let date2 = dates[1];

    let dateShort1 = this.getDateString(date1.getFullYear(), date1.getMonth() + 1, date1.getDate());
    let dateShort2 = this.getDateString(date2.getFullYear(), date2.getMonth() + 1, date2.getDate());

    this.selectedDates[this.getDateKey('calendar1', dateShort1)] = { dayNo: date1.getDate(), date: dateShort1, isDifferentMonth: false, isDisabled: false, isSelected: true, isCurrentDate: this.isCurrentDate(dateShort1) };
    this.selectedDates[this.getDateKey('calendar2', dateShort2)] = { dayNo: date1.getDate(), date: dateShort2, isDifferentMonth: false, isDisabled: false, isSelected: true, isCurrentDate: this.isCurrentDate(dateShort2) };

    this.updateSelectedDates();
    this.updateSelecteDateCalendar();

    this.DatePeriodChanged.emit(this.selectedPeriod);

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
    this.updateSelecteDateCalendar();

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
    this.updateSelecteDateCalendar();
  }
  onCancelDateRangeFilter() {
    this.CancelDateSelection.emit();
  }
  onApplyFilterByDateRange() {
    this.selectedPeriod = settings_appointment_search_by_date_option.daterange;
    this.DateSelectionChanged.emit(this.selectedDates);
  }
  get isValidDateRange() {
    return Object.keys(this.selectedDates).length == 2;
  }
  onSelectedDay(calendar: string, day: IDay) {

    const key = this.getDateKey(calendar, day.date);
    this.selectedDates[key] = day;

    const keys = Object.keys(this.selectedDates);

    if (keys.length > 2) {
      this.selectedDates[keys[0]].isSelected = false;
      this.selectedDates[keys[1]].isSelected = false;
      delete this.selectedDates[keys[0]];
      delete this.selectedDates[keys[1]];
    }
    else if (keys.length == 2) {
      let date1 = new Date(this.selectedDates[keys[0]].date);
      let date2 = new Date(this.selectedDates[keys[1]].date);
      if (date1 > date2) {
        this.selectedDates[keys[0]].isSelected = false;
        delete this.selectedDates[keys[0]];
      }

    }
    this.updateSelecteDateCalendar();
  }

  private getDateKey(calendar: string, date: string) {
    return `${calendar}:${date}`;
  }

  private updateCalendar() {
    this.calendar1MonthName = `${this.months[this.calendar1Month - 1]} ${this.calendar1Year}`;
    this.calendar2MonthName = `${this.months[this.calendar2Month - 1]} ${this.calendar2Year}`;
    this.updateCalendar1();
    this.updateCalendar2();
  }
  private updateCalendar1() {
    this.calendar1 = this.generateCalendar(this.calendar1Year, this.calendar1Month);
  }

  private updateCalendar2() {
    this.calendar2 = this.generateCalendar(this.calendar2Year, this.calendar2Month);
  }

  private generateCalendar(year: number, month: number): IDay[][] {
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
          week.push({ dayNo: daysInMonthPrevious - (firstDay - (j + 1)), date: previousMonthDateString, isDifferentMonth: true, isDisabled: false, isSelected: false, isCurrentDate: this.isCurrentDate(previousMonthDateString) }); // Placeholder for days before the 1st of the month
        } else if (currentDate <= daysInMonth) {
          let shortDateString = this.getDateString(year, month, currentDate);
          week.push({ dayNo: currentDate, date: shortDateString, isDifferentMonth: false, isDisabled: false, isSelected: false, isCurrentDate: this.isCurrentDate(shortDateString) });
          currentDate++;
        } else {
          let nextMonthDateString = this.getDateString(nextMonthYear, nextMonth, nextMonthDate);
          week.push({ dayNo: nextMonthDate, date: nextMonthDateString, isDifferentMonth: true, isDisabled: false, isSelected: false, isCurrentDate: this.isCurrentDate(nextMonthDateString) }); // Placeholder for days after the end of the month
          nextMonthDate++;
        }
      }
      calendar.push(week);
    }

    return calendar;
  }


  private updateSelecteDateCalendar() {
    this.calendar1.forEach((week) => {
      week.forEach((weekDay) => {
        let dateKey1 = this.getDateKey('calendar1', weekDay.date);
        let dateKey2 = this.getDateKey('calendar2', weekDay.date);
        if (this.selectedDates[dateKey1] || this.selectedDates[dateKey2]) {
          weekDay.isSelected = true;
        } else {
          weekDay.isSelected = false;
        }
      });
    });

    this.calendar2.forEach((week) => {
      week.forEach((weekDay) => {
        let dateKey1 = this.getDateKey('calendar1', weekDay.date);
        let dateKey2 = this.getDateKey('calendar2', weekDay.date);
        if (this.selectedDates[dateKey1] || this.selectedDates[dateKey2]) {
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
  private isCurrentDate(date: string) {
    let currentDate = new Date();
    let selectedDate = new Date(date);
    return currentDate.getFullYear() == selectedDate.getFullYear() && currentDate.getMonth() == selectedDate.getMonth() && currentDate.getDate() == selectedDate.getDate();
  }
  private updateSelectedDates() {

    let keys = Object.keys(this.selectedDates);
    keys.forEach((key) => {
      let calendar = key.split(':')[0];
      let date = key.split(':')[1];

      this.calendar1.forEach((week) => {
        week.forEach((weekDay) => {
          if (weekDay.date == date) {
            weekDay.isSelected = true;
            this.selectedDates[key] = weekDay;
          }
        });
      });
      this.calendar2.forEach((week) => {
        week.forEach((weekDay) => {
          if (weekDay.date == date) {
            weekDay.isSelected = true;
            this.selectedDates[key] = weekDay;
          }
        });
      });
    });
  }
  private resetSelectedDates() {
    this.selectedDates = {};
    this.calendar1.forEach((week) => {
      week.forEach((weekDay) => {
        weekDay.isSelected = false;
      });
    });
    this.calendar2.forEach((week) => {
      week.forEach((weekDay) => {
        weekDay.isSelected = false;
      });
    });
  }
}
