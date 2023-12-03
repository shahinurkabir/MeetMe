
using MediatR;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Core.Constants;
using MeetMe.Core.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Queries
{
    public class TimeSlotRangeQuery : IRequest<List<TimeSlotRangeDto>>
    {
        public string EventTypeSlug { get; set; }=null!;
        public string TimeZone { get; set; } = null!;
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
    }

    public class TimeSlotRangeQueryHandler : IRequestHandler<TimeSlotRangeQuery, List<TimeSlotRangeDto>>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public TimeSlotRangeQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<List<TimeSlotRangeDto>> Handle(TimeSlotRangeQuery request, CancellationToken cancellationToken)
        {
            var (dateFromUTC, dateToUTC) = ConvertToUniversalTime(request.FromDate, request.ToDate, request.TimeZone);

            var eventTypeEntity = await persistenceProvider.GetEventTypeBySlug(request.EventTypeSlug);
            
            if (eventTypeEntity == null) throw new MeetMeException("Event type not found");

            var listOfAppointmentsBooked = await persistenceProvider.GetAppointmentListByEventType(eventTypeEntity.Id, dateFromUTC, dateToUTC);
            
            var bufferTimeInMinute = eventTypeEntity!.BufferTimeAfter;
            var meetingDuration = eventTypeEntity.Duration;

            var timeZoneInfo_User = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);
            var timeZoneInfo_Calendar = TimeZoneInfo.FindSystemTimeZoneById(eventTypeEntity.TimeZone);

            var (dateFrom_User, dateTo_User) = ConvertToUserTime(request.FromDate, request.ToDate, timeZoneInfo_User, timeZoneInfo_Calendar);

            var listTimeSlots = GenerateTimeSlots(dateFrom_User, dateTo_User, meetingDuration, bufferTimeInMinute, eventTypeEntity.EventTypeAvailabilityDetails);
           
            var result = FinalizeScheduleDates(timeZoneInfo_User, meetingDuration, listOfAppointmentsBooked!, ref listTimeSlots);

            return result;
        }

        private static (DateTime, DateTime) ConvertToUniversalTime(string fromDate, string toDate, string timeZoneId)
        {
            var tempFromDate = DateTime.Parse(fromDate);
            var tempToDate = DateTime.Parse(toDate);
            var tempFromUTC = TimeZoneInfo.ConvertTimeToUtc(tempFromDate, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
            var tempToUTC = TimeZoneInfo.ConvertTimeToUtc(tempToDate, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
            return (tempFromUTC, tempToUTC);
        }

        private static (DateTime, DateTime) ConvertToUserTime(string fromDate, string toDate, TimeZoneInfo userTimeZone, TimeZoneInfo calendarTimeZone)
        {
            var tempFromDate = DateTime.Parse(fromDate);
            var tempToDate = DateTime.Parse(toDate);
            var dateFrom_User = TimeZoneInfo.ConvertTime(tempFromDate, userTimeZone);
            var dateTo_User = TimeZoneInfo.ConvertTime(tempToDate, userTimeZone);

            var dateFrom_Calendar = TimeZoneInfo.ConvertTime(dateFrom_User, calendarTimeZone).AddDays(-1);
            var dateTo_Calendar = TimeZoneInfo.ConvertTime(dateTo_User, calendarTimeZone).AddDays(1);

            return (dateFrom_Calendar, dateTo_Calendar);
        }

        private List<TimeSlot> GenerateTimeSlots(DateTimeOffset dateFrom_Calendar, DateTimeOffset dateTo_Calendar, int meetingDuration, int bufferTime, List<EventTypeAvailabilityDetail> availabilityDetails)
        {
            var result = new List<TimeSlot>();
            var dateRunning = dateFrom_Calendar;

            while (dateRunning <= dateTo_Calendar)
            {
                var weekDay = dateRunning.DayOfWeek;
                var schedulesForDate = GetSchedulesForDate(availabilityDetails, dateRunning, weekDay);

                if (schedulesForDate != null && schedulesForDate.Any())
                {
                    foreach (var scheduleItem in schedulesForDate)
                    {
                        var (dayStartFromInMinutes, dayEndFromInMinutes) = (scheduleItem.From, scheduleItem.To);
                        var timeSlots = GenerateTimeSlotForDate(dateRunning, bufferTime, meetingDuration, dayStartFromInMinutes, dayEndFromInMinutes).ToList();
                        result.AddRange(timeSlots);
                    }
                }

                dateRunning = dateRunning.AddDays(1);
            }

            return result;
        }

        private static List<EventTypeAvailabilityDetail> GetSchedulesForDate(List<EventTypeAvailabilityDetail> availabilityDetails, DateTimeOffset scheduleDate, DayOfWeek weekDay)
        {
            var listScheduleTimesPerDay = availabilityDetails
                .Where(x => x.DayType == Events.SCHEDULE_DATETYPE_WEEKDAY && x.Value == weekDay.ToString())
                .ToList();
            var listScheduleTimesPerDate = availabilityDetails
                .Where(x => x.DayType == Events.SCHEDULE_DATETYPE_DATE && DateTime.Parse(x.Value) == scheduleDate.Date)
                .ToList();

            if (listScheduleTimesPerDate != null && listScheduleTimesPerDate.Any())
            {
                listScheduleTimesPerDay = listScheduleTimesPerDate;
            }

            return listScheduleTimesPerDay;
        }

        private static IEnumerable<TimeSlot> GenerateTimeSlotForDate(DateTimeOffset calendarDate, double bufferTimeInMinutes, int meetingDuration, double dayStartFromInMinutes, double dayEndInMinutes)
        {
            var dayStartTime = DateTimeOffset.Parse(calendarDate.ToString("yyyy-MM-dd ")).AddMinutes(dayStartFromInMinutes);
            var dayEndTime = DateTimeOffset.Parse(calendarDate.ToString("yyyy-MM-dd ")).AddMinutes(dayEndInMinutes);

            while (dayStartTime < dayEndTime)
            {
                yield return new TimeSlot { StartDateTime = dayStartTime.ToUniversalTime() };
                dayStartTime = dayStartTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinutes);
            }
        }

        private List<TimeSlotRangeDto> FinalizeScheduleDates(TimeZoneInfo timeZoneInfo_User, int meetingDuration, List<AppointmentDetailsDto> appointmentsBooked, ref List<TimeSlot> listTimeSlots)
        {
            listTimeSlots = listTimeSlots.Where(e => e.StartDateTime > DateTimeOffset.UtcNow).ToList();

            foreach (var appointment in appointmentsBooked)
            {
                listTimeSlots.RemoveAll(e => appointment.StartTimeUTC >= e.StartDateTime.UtcDateTime &&
                                            appointment.EndTimeUTC <= e.StartDateTime.AddMinutes(meetingDuration).UtcDateTime);
            }

            listTimeSlots.ForEach(e => e.StartDateTime = TimeZoneInfo.ConvertTime(e.StartDateTime, timeZoneInfo_User));

            var listDatesGroup = listTimeSlots.GroupBy(e => e.StartDateTime.DateTime.Date);

            var result = new List<TimeSlotRangeDto>();

            foreach (var dateGroup in listDatesGroup)
            {
                var date = dateGroup.Key;
                var listDate = dateGroup.ToList();
                listDate = listDate.Where(e => e.StartDateTime.DateTime.Date == date).ToList();

                var eventTimeCalendar = new TimeSlotRangeDto
                {
                    Date = date.ToString("yyyy-MMM-dd"),
                    Slots = listDate
                };
                result.Add(eventTimeCalendar);
            }

            return result;
        }
    }
}


