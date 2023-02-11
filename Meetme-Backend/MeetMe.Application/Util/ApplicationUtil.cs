using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Constant;

namespace MeetMe.Application.Util
{
    public class ApplicationUtil
    {
        public static List<DailyTimeSchedule> ConvertScheduleTimeAs(TimeSpan timeZoneOffset, List<DailyTimeSchedule> schedules)
        {
            List<DailyTimeSchedule> dailyTimeSchedules = new List<DailyTimeSchedule>();

            foreach (var schedule in schedules)
            {
                dailyTimeSchedules.Add(new DailyTimeSchedule
                {
                    Type = schedule.Type,
                    Day = schedule.Day,
                    Date = schedule.Date,
                    From = schedule.From + timeZoneOffset.TotalMinutes,
                    To = schedule.To + timeZoneOffset.TotalMinutes

                });
            }

            return dailyTimeSchedules;

        }

        public static void GenerateCalender(string today, string fromDate, string toDate, string availTimeZone, string userTimeZone)
        {
            availTimeZone = "Bangladesh Standard Time";
            userTimeZone = "Fiji Standard Time";
            var availTimeFrom = "09:00";
            var availTimeTo = "17:00";
            var meetingDuration = 30;
            var bufferTime = 15;
            var timeZoneCalendar = TimeZoneInfo.FindSystemTimeZoneById(availTimeZone);
            var timeZoneUser = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);

            var currentTimeUtc = DateTime.UtcNow;
            var currentTimeUser = TimeZoneInfo.ConvertTime(currentTimeUtc, timeZoneUser);
            var currentTimeCalender = TimeZoneInfo.ConvertTime(currentTimeUtc, timeZoneCalendar);

            var calenderTimeFrom = currentTimeCalender.Date.Add(TimeSpan.Parse(availTimeFrom));
            var calenderTimeTo = currentTimeCalender.Date.Add(TimeSpan.Parse(availTimeTo));

            var calenderAvailFromUtc = TimeZoneInfo.ConvertTimeToUtc(calenderTimeFrom, timeZoneCalendar);
            var calenderAvailToUtc = TimeZoneInfo.ConvertTimeToUtc(calenderTimeFrom, timeZoneCalendar);

            var calendarAvailFromTimePlan = calenderAvailFromUtc.TimeOfDay;
            var calendarAvailToTimeSpan = calenderAvailToUtc.TimeOfDay;

            var daysInMonthUser = DateTime.DaysInMonth(currentTimeCalender.Year, currentTimeCalender.Month);

            var index = 0;

            var tempScheduleDate = calenderAvailFromUtc;

            while (index < daysInMonthUser)
            {
                index++;

                var slotIndex = 0;
                // var scheduleStartTime=
                while (tempScheduleDate < calenderAvailToUtc)
                {
                    if (tempScheduleDate <= currentTimeUser) continue;

                    slotIndex++;
                    var scheduleTimeString = tempScheduleDate.ToString("hh:mm tt");

                    Console.WriteLine("Date :{0} Schedule {1} Time is :{2}", tempScheduleDate.Date.ToString("dd-MMM-yyyy"), slotIndex, scheduleTimeString);
                    tempScheduleDate.AddMinutes(meetingDuration + bufferTime);

                }//Slots loop

            }// Date loop
        }
        public static Dictionary<DateTime, IEnumerable<DateTime>> GenerateCalenderDateTimeSlots(DateTime dateFrom, DateTime dateTo, string timeFromAvil, string timeToAvail, int meetingDurationInMinute, int bufferTimeInMinute)
        {
            var dateTimeSlots = new Dictionary<DateTime, IEnumerable<DateTime>>();

            var dateForTimeSlot = dateFrom;

            while (dateTo <= dateForTimeSlot)
            {
                var dateString = dateForTimeSlot.ToString("yyyy-mm-dd");

                var timeSlots = GenerateTimeSlots(dateForTimeSlot, timeFromAvil, timeToAvail, meetingDurationInMinute, bufferTimeInMinute);

                dateTimeSlots[dateForTimeSlot] = timeSlots;

                dateForTimeSlot.AddDays(1);
            }

            return dateTimeSlots;
        }

        static IEnumerable<DateTime> GenerateTimeSlots(DateTime calenderDate, string availabilityFrom, string availabilityEnd, int meetingDuration, int bufferTimeInMinute)
        {
            var fromDateTime = DateTime.Parse(calenderDate.ToString("yyyy-MM-dd ") + availabilityFrom);
            var toDateTime = DateTime.Parse(calenderDate.ToString("yyyy-MM-dd ") + availabilityEnd);

            while (toDateTime > fromDateTime)
            {
                fromDateTime = fromDateTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinute);

                yield return fromDateTime;

            }

        }

        public string GetDateCrossTimeZone(string date, string timeZoneFrom, string timeZoneTo)
        {
            return "";
        }

        public static List<DateTime> GenerateTimeSpotsInDate(
            DateTime spotDate,
            double offsetLeft,
            double offsetRight,
            int meetingDuration,
            int bufferTimeBefore,
            int bufferTimeAfter,
            List<DailyTimeSchedule> timeSchedules
            )
        {
            var slotTimes = new List<DateTime>();

            var spotDay = spotDate.ToString("dddd");

            var schedulesInDay = timeSchedules.Where(e =>
                e.Type == Constants.Events.SCHEDULE_DATETYPE_WEEKDAY &&
                e.Day == spotDay).OrderBy(e => e.From)
                .ToList();

            if (schedulesInDay.Count == 0) return slotTimes;

            foreach (var schedule in schedulesInDay)
            {
                var timeSlotStartTime = spotDate.Add(new TimeSpan(0, (int)schedule.From, 0));
                var timeSlotEndTime = spotDate.Add(new TimeSpan(0, (int)schedule.To, 0));

                var tempScheduleTimeSlot = timeSlotStartTime.AddMinutes(bufferTimeBefore);

                while (tempScheduleTimeSlot < timeSlotEndTime)
                {
                    var slotTimeInMinutes = tempScheduleTimeSlot.TimeOfDay.TotalMinutes;

                    if (slotTimeInMinutes >= timeSlotStartTime.TimeOfDay.TotalMinutes && slotTimeInMinutes < timeSlotEndTime.TimeOfDay.TotalMinutes)
                        slotTimes.Add(tempScheduleTimeSlot.ToUniversalTime());

                    tempScheduleTimeSlot = tempScheduleTimeSlot.AddMinutes(meetingDuration + bufferTimeAfter);
                }
            }

            return slotTimes;
        }


        public static List<EventTypeAvailabilityDetail> GetDefaultWeeklySchedule(Guid eventTypeId)
        {
            var dayStartInMinutes = TimeSpan.Parse(Constants.Events.MEETING_FROM_TIMESPAN).TotalMinutes;
            var dayEndINMinutes = TimeSpan.Parse(Constants.Events.MEETING_TO_TIMESPAN).TotalMinutes;

            var listWeekDaysConfig = new List<EventTypeAvailabilityDetail>();

            short stepId = 0;
            foreach (KeyValuePair<int, string> weekDay in Constants.WeekDays)
            {
                listWeekDaysConfig.Add(new EventTypeAvailabilityDetail
                {
                    Id = Guid.NewGuid(),
                    AvailabilityId= eventTypeId,
                    Type = Constants.Events.SCHEDULE_DATETYPE_WEEKDAY,
                    Day = weekDay.Value,
                    From = dayStartInMinutes,
                    To = dayEndINMinutes,
                    StepId=stepId
                });
                stepId++;
            }

            return listWeekDaysConfig;
        }

        public static List<EventTypeQuestion> GetDefaultQuestion()
        {
            var questions = new List<EventTypeQuestion>()
            {
                new EventTypeQuestion
                {
                    Name="Name",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=1 ,
                    RequiredYN=true, 
                },
                 new EventTypeQuestion
                {
                    Name="Email",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=2 ,
                    RequiredYN=true,
                }
            };

            return questions;
        }

        public static EventTypeAvailability GetDefaultAvailability(Guid eventTypeId, int timeZoneId)
        {
            var availability = new EventTypeAvailability
            {
                Id = eventTypeId,
                TimeZoneId=timeZoneId,
                DateForwardKind = Constants.Events.ForwandDateKInd.Moving.ToString(),
                ForwardDuration = Constants.Events.ForwardDuration,
                Duration = Constants.Events.MeetingDuration,
                BufferTimeBefore = Constants.Events.BufferTimeDuration,
                BufferTimeAfter = Constants.Events.BufferTimeDuration,
                AvailabilityDetails =GetDefaultWeeklySchedule(eventTypeId)
            };

            return availability;
        }
        
        //public static List<CalendarDateInfo> GetDateOffsetsForTimeZoneCoverage(DateTime calendarDate, DateTime currentTimeInCalender, double offsetDiffInMinutes)
        //{
        //    var listDates = new List<CalendarDateInfo>();

        //    if (offsetDiffInMinutes == 0)
        //    {
        //        listDates.Add(new CalendarDateInfo
        //        {
        //            CalendarDate = calendarDate.Date,
        //            CurrentTime = currentTimeInCalender,
        //            OffsetLeft = calendarDate.Date == currentTimeInCalender.Date ? currentTimeInCalender.TimeOfDay.TotalMinutes : 0,
        //            OffsetRight = TimeSpan.Parse("23:59:59").TotalMinutes
        //        });
        //    }
        //    else if (offsetDiffInMinutes < 0)
        //    {
        //        listDates.Add(new CalendarDateInfo
        //        {
        //            CalendarDate = calendarDate.Date.AddDays(-1),
        //            CurrentTime = currentTimeInCalender,
        //            OffsetLeft = TimeSpan.Parse("23:59:59").TotalMinutes + offsetDiffInMinutes,
        //            OffsetRight = TimeSpan.Parse("23:59:59").TotalMinutes
        //        });

        //        listDates.Add(new CalendarDateInfo
        //        {
        //            CalendarDate = calendarDate.Date,
        //            CurrentTime = currentTimeInCalender,
        //            OffsetLeft = 0,
        //            OffsetRight = TimeSpan.Parse("23:59:59").TotalMinutes + offsetDiffInMinutes
        //        });
        //    }
        //    else if (offsetDiffInMinutes > 0)
        //    {
        //        listDates.Add(new CalendarDateInfo
        //        {
        //            CalendarDate = calendarDate.Date,
        //            CurrentTime = currentTimeInCalender,
        //            OffsetLeft = offsetDiffInMinutes,
        //            OffsetRight = TimeSpan.Parse("23:59:59").TotalMinutes
        //        });

        //        listDates.Add(new CalendarDateInfo
        //        {
        //            CalendarDate = calendarDate.Date.AddDays(1),
        //            CurrentTime = currentTimeInCalender,
        //            OffsetLeft = TimeSpan.Parse("00:00:01").TotalMinutes,
        //            OffsetRight = offsetDiffInMinutes
        //        });
        //    }


        //    return listDates;
        //}
    }
}
