import { Injectable } from "@angular/core";

export const day_of_week: string[] = [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday"];
export const month_of_year: string[] = [
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

export const meeting_day_type_weekday = 'weekday';
export const meeting_day_type_date = 'date';
export const default_meeting_duration = 30;
export const default_meeting_forward_Duration_inDays = 60 * 24 * 60;
export const default_meeting_buffertime = 15;
export const default_startTime_minutes = 60 * 9 // 9:00am 
export const default_endTime_Minutes = 60 * 17;// 5:00pm 

