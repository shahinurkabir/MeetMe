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

export const time_Slots_List: string[] = [
    "12:00am", "12:15am", "12:30am", "12:45am", "1:00am", "1:15am", "1:30am", "1:45am",
    "2:00am", "2:15am", "2:30am", "2:45am", "3:00am", "3:15am", "3:30am", "3:45am",
    "4:00am", "4:15am", "4:30am", "4:45am", "5:00am", "5:15am", "5:30am", "5:45am",
    "6:00am", "6:15am", "6:30am", "6:45am", "7:00am", "7:15am", "7:45am", "8:00am",
    "8:15am", "8:30am", "8:45am", "9:00am", "9:15am", "9:30am", "9:45am", "10:00am",
    "10:15am", "10:30am", "10:45am", "11:00am", "11:15am", "11:30am", "11:45am",
    "12:00pm", "12:15pm", "12:30pm", "12:45pm", "1:00pm", "1:15pm", "1:30pm", "1:45pm",
    "2:00pm", "2:15pm", "2:30pm", "2:45pm", "3:00pm", "3:15pm", "3:30pm", "3:45pm",
    "4:00pm", "4:15pm", "4:30pm", "4:45pm", "5:00pm", "5:15pm", "5:30pm", "5:45pm",
    "6:00pm", "6:15pm", "6:30pm", "6:45pm", "7:00pm", "7:15pm", "7:45pm", "8:00pm",
    "8:15pm", "8:30pm", "8:45pm", "9:00pm", "9:15pm", "9:30pm", "9:45pm", "10:00pm",
    "10:15pm", "10:30pm", "10:45pm", "11:00pm", "11:15pm", "11:30pm", "11:45pm", "12:00pm"
  ];


export const meeting_day_type_weekday = 'weekday';
export const meeting_day_type_date = 'date';
export const default_meeting_duration = 30;
export const default_meeting_forward_Duration_inDays = 60 * 24 * 60;
export const default_meeting_buffertime = 15;
export const default_startTime_minutes = 60 * 9 // 9:00am 
export const default_endTime_Minutes = 60 * 17;// 5:00pm 

