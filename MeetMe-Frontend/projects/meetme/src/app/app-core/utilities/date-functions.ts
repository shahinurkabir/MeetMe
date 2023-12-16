import { settings_month_of_year } from "./constants-data";

export class DateFunction {
    static isDayPast(date: Date): boolean {
        let currentDateTime = new Date();
        let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
        return currentDate.getTime() > date.getTime();
    }
    static isDayCurrent(date: Date): boolean {
        let currentDateTime = new Date();
        let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
        return currentDate.getTime() == date.getTime();
    }
    static isMonthCurrent(date: Date): boolean {
        let currentDateTime = new Date();
        return currentDateTime.getFullYear() == date.getFullYear()
            && currentDateTime.getMonth() == date.getMonth();
    }

    static convertTimeZone(date: Date | string, timeZone: string): Date {
        return new Date((typeof date === "string" ? new Date(date) : date).toLocaleString("en-US", { timeZone: timeZone }));
    }
    static getTimeWithAMPM(date: Date, is24HourFormat: boolean, timeZone: string): string {
        if (!is24HourFormat)
            return new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: 'numeric', timeZone: timeZone }).format(date);
        else
            return new Intl.DateTimeFormat('en-US', { hour12: false, hour: 'numeric', minute: 'numeric', timeZone: timeZone }).format(date);
    }

    static getDaysInMonth(year: number, month: number): number {
        return new Date(year, month + 1, 0).getDate();
    }

    static getDateString(year: number, month: number, day: number): string {
        return `${year}-${month + 1}-${day}`;
    }

    static getCurrentDateInTimeZone(timeZone: string): Date {
        return new Date(new Date().toLocaleString("en-US", { timeZone }));
    }

    static getDaysInRange(startDate: Date, endDate: Date): number[] {
        const daysInRange: number[] = [];
        const currentDate = new Date(startDate);

        while (currentDate <= endDate) {
            daysInRange.push(currentDate.getDate());
            currentDate.setDate(currentDate.getDate() + 1);
        }

        return daysInRange;
    }
    static getFormattedDate(year: number, month: number, dayNo: number): string {

        let shortMonth = settings_month_of_year[month].substring(0, 3);
        let dayNoString = "0" + dayNo.toString();
        dayNoString = dayNoString.substring(dayNoString.length - 2, dayNoString.length);
        let date = `${year}-${shortMonth}-${dayNoString}`;

        return date;
    }
}

// export const date = {
//     isDayPast: function (date: Date) {
//         let currentDateTime = new Date();
//         let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
//         return currentDate.getTime() > date.getTime();
//     },
//     isDayCurrent: function (date: Date) {
//         let currentDateTime = new Date();
//         let currentDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate())
//         return currentDate.getTime() == date.getTime();
//     },
//     isMonthCurrent: function (date: Date) {
//         let currentDateTime = new Date();
//         return currentDateTime.getFullYear() == date.getFullYear()
//             && currentDateTime.getMonth() == date.getMonth()
//     }
// }

// export function convertTimeZone(date: Date | string, timeZone: string): Date {
//     return new Date((typeof date === "string" ? new Date(date) : date).toLocaleString("en-US", { timeZone: timeZone }));
// }
// export function getTimeWithAMPM(date: Date, is24HourFormat: boolean, timeZone: string): string {
//     if (!is24HourFormat)
//         return new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: 'numeric', timeZone: timeZone }).format(date);
//     else
//         return new Intl.DateTimeFormat('en-US', { hour12: false, hour: 'numeric', minute: 'numeric', timeZone: timeZone }).format(date);
// }

// export function getDaysInMonth(year: number, month: number): number {
//     return new Date(year, month + 1, 0).getDate();
// }

// export function getDateString(year: number, month: number, day: number): string {
//     return `${year}-${month + 1}-${day}`;
// }

// export function getCurrentDateInTimeZone(timeZone: string): Date {
//     return new Date(new Date().toLocaleString("en-US", { timeZone }));
// }

// export function getDaysInRange(startDate: Date, endDate: Date): number[] {
//     const daysInRange: number[] = [];
//     const currentDate = new Date(startDate);

//     while (currentDate <= endDate) {
//         daysInRange.push(currentDate.getDate());
//         currentDate.setDate(currentDate.getDate() + 1);
//     }

//     return daysInRange;
// }