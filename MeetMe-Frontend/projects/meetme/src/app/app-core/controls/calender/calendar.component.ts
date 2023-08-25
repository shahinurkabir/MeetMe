import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { day_of_week, month_of_year } from '../../utilities/default-data';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss']
})
export class CalendarComponent implements OnInit {
  
  @Output() handlerDateClick = new EventEmitter()
  @Output() handlerMonthChange = new EventEmitter();
  @Input() selectedDates: { [id: string]: string | undefined } = {};
  @Input() allowMultipleSelection: boolean = true;
  @Input() minDate: Date | undefined = undefined;
  @Input() maxDate: Date | undefined = undefined;
  selectedMonth: number = 0
  selectedYear: number = 2023
  selectedYearMonth: string = "";
  weekDays = day_of_week;
  days_in_month: { [weekNo: string]: IDay[] } = {};
  timeZoneMame: string | undefined = undefined;

  constructor() {
    this.timeZoneMame = Intl.DateTimeFormat().resolvedOptions().timeZone;
    let localDate = new Date(new Date().toLocaleString("en-US", { timeZone: this.timeZoneMame }));
    this.resetYearMonthFromDate(localDate);
    this.updateCalendar();
   }

  ngOnInit(): void {
    //this.resetCalendar();
  }
  moveTo(selectedYear: number, selectedMonth: number) {
    this.selectedYear = selectedYear;
    this.selectedMonth = selectedMonth;
    this.updateCalendar();
  }
  onNextMonth() {
    if (this.selectedMonth + 1 > 11) {
      this.selectedMonth = 0;
      this.selectedYear++
    }
    else {
      this.selectedMonth++;
    }
    this.updateCalendar();
  }

  onPreviousMonth() {
    if (this.selectedMonth - 1 < 0) {
      this.selectedMonth = 11;
      this.selectedYear--
    }
    else {
      this.selectedMonth--;
    }

    this.updateCalendar();

  }

  updateCalendar() {
    let calendarDay = new Date(this.selectedYear, this.selectedMonth, 1, 23, 59, 59, 999);

    this.selectedYearMonth = month_of_year[this.selectedMonth] + " " + this.selectedYear;
    this.days_in_month = {};

    for (let i = 0; i < 7; i++) {
      let weekDays: IDay[] = [];
      for (let j = 0; j < 6; j++) {
        weekDays.push({ dayNo: 0, isSelected: false, date: "", isDisabled: false, isCurrentDate: false })
      }
      this.days_in_month[i] = weekDays;
    }

    calendarDay.setDate(1);

    let weekNo = 0;

    let currentDate = this.getCurrentDateByTimeZone();
    let currentTimeByTimeZone = currentDate.getTime();

    for (let i = 0; i < 31; i++) {

      if (calendarDay.getDate() < i) break;
      let weekDay = calendarDay.getDay();
      let dayNo = i + 1
      let shortDateString = this.getShortDateString(dayNo);
      let isPastDate =  currentTimeByTimeZone > calendarDay.getTime(); 
      let isCurrentDate =this.isCurrentMonth() && currentDate.getDate() == calendarDay.getDate() ;
      this.days_in_month[weekNo][weekDay] = { dayNo: dayNo, date: shortDateString, isDisabled: isPastDate, isSelected: false, isCurrentDate: isCurrentDate };

      if (weekDay == 6) {
        weekNo += 1
      }

      calendarDay.setDate(calendarDay.getDate() + 1)

    }

    this.handlerMonthChange.emit({ year: this.selectedYear, month: this.selectedMonth });
  }

  onClickDay(weekDay: IDay) {

    if (!this.allowMultipleSelection) {
      for (let week in this.days_in_month) {
        let listDaysInWeek = this.days_in_month[week];
        listDaysInWeek.forEach(day => day.isSelected = false);
      }
      weekDay.isSelected = true;
      this.selectedDates = {};
      this.selectedDates[weekDay.date] = weekDay.date;
    }
    else {
      weekDay.isSelected = !weekDay.isSelected;

      if (this.selectedDates[weekDay.date])
        delete this.selectedDates[weekDay.date];
      else
        this.selectedDates[weekDay.date] = weekDay.date;
    }


    this.handlerDateClick.emit(this.selectedDates);
  }

  isCurrentMonth(): boolean {
    let currentTimeByTimeZone = this.getCurrentDateByTimeZone();
    return this.selectedYear == currentTimeByTimeZone.getFullYear()
      && this.selectedMonth == currentTimeByTimeZone.getMonth();
  }

  resetCalendar(defaultDate?: Date) {

    if (!defaultDate) {
      let currentDate = this.getCurrentDateByTimeZone();
      this.resetYearMonthFromDate(currentDate);
      this.updateCalendar();
    }
    else {
      this.selectedYear = defaultDate.getFullYear();
      this.selectedMonth = defaultDate.getMonth();
      let defaultDayNo = defaultDate.getDate();

      this.updateCalendar();


      for (let week in this.days_in_month) {
        let listDaysInWeek = this.days_in_month[week];
        listDaysInWeek.forEach(day => day.isSelected = (day.dayNo == defaultDayNo!));
      }

      let date = this.getShortDateString(defaultDayNo);
      this.selectedDates[date] = date;
      this.handlerDateClick.emit(this.selectedDates);
    }
  }
  resetTimeZone(timezone: string) {
    this.timeZoneMame = timezone;
    let localDate = this.getCurrentDateByTimeZone();
    this.resetYearMonthFromDate(localDate);
    this.updateCalendar();
  }
  resetYearMonthFromDate(date: Date) {
    this.selectedYear = date.getFullYear();
    this.selectedMonth = date.getMonth();
    //this.selectedDates = {};
  }
  disableDays(days: number[]) {
    for (let week in this.days_in_month) {
      let listDaysInWeek = this.days_in_month[week];
      listDaysInWeek.forEach(day => {
        if (day.dayNo > 0 && days.indexOf(day.dayNo) > -1) {
          day.isDisabled = true;
        }
      });
    }
  }
  private getCurrentDateByTimeZone(): Date {
    return new Date(new Date().toLocaleString("en-US", { timeZone: this.timeZoneMame }));
  }
  private getShortDateString(dayNo: number): string {

    let shortMonth = month_of_year[this.selectedMonth].substring(0, 3);
    let dayNoString = "0" + dayNo.toString();
    dayNoString = dayNoString.substring(dayNoString.length - 2, dayNoString.length);
    let date = `${this.selectedYear}-${shortMonth}-${dayNoString}`;

    return date;
  }

}
interface IDay {
  dayNo: number,
  date: string,
  isDisabled: boolean,
  isSelected: boolean
  isCurrentDate: boolean,
}