export function convertTimeZoneLocalTime(date: Date, is24HourFormat: boolean, timeZone: string): string {
    if (!is24HourFormat)
        return date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', timeZone: timeZone });
    else
        return date.toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit', timeZone: timeZone });
}
