using MediatR;
using MeetMe.Application.Dtos;
using MeetMe.Application.EventTypes.Calendar.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Calendar
{
    public class EventTimeCalendarCommand : IRequest<List<EventTimeCalendar>>
    {
        public Guid EventTypeId { get; set; }
        public string TimeZone { get; set; } = null!;
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
    }

    public class EventTimeCalendarCommandHandler : IRequestHandler<EventTimeCalendarCommand, List<EventTimeCalendar>>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IEventTypeAvailabilityDetailRepository eventTypeAvailabilityDetailRepository;

        public EventTimeCalendarCommandHandler(
            IEventTypeRepository eventTypeRepository,
            IAvailabilityRepository availabilityRepository,
            IEventTypeAvailabilityDetailRepository eventTypeAvailabilityDetailRepository
            )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.availabilityRepository = availabilityRepository;
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
        }
        public async Task<List<EventTimeCalendar>> Handle(EventTimeCalendarCommand request, CancellationToken cancellationToken)
        {
            var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.EventTypeId);
            var calendarTimeZone = eventTypeEntity.TimeZone;
            var bufferTime = eventTypeEntity.BufferTimeAfter;
            var meetingDuration = eventTypeEntity.Duration;

            //if (availabilityId.HasValue)
            //{
            //    listAvailability = (await availabilityRepository.GetScheduleById(availabilityId.Value)).Details.Select(e => new AvailabilityDetailDto
            //    {
            //        DayType = e.DayType,
            //        Value = e.Value,
            //        StartFrom = e.From,
            //        EndAt = e.To
            //    }).ToList();
            //}
            //else
            //{
            //    listAvailability = (await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityDetailByEventId(eventTypeEntity.Id)).Select(e => new AvailabilityDetailDto
            //    {
            //        DayType = e.DayType,
            //        Value = e.Value,
            //        StartFrom = e.From,
            //        EndAt = e.To
            //    }).ToList();
            //}
            var availabilityList = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityDetailByEventId(eventTypeEntity.Id);

            var eventTimeCalendarDetails = GetCalendarTimeSlots(request.TimeZone, calendarTimeZone, request.FromDate, request.ToDate, bufferTime, meetingDuration, availabilityList);

            return eventTimeCalendarDetails;

        }

        private List<EventTimeCalendar> GetCalendarTimeSlots(
            string timezoneUser, string timezoneCalendar, 
            string dateFrom, string dateTo, int bufferTime, 
            int meetingDuration, List<EventTypeAvailabilityDetail> scheduleList
            )
        {
            var result = new List<EventTimeCalendar>();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneUser);
            var calendarTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneCalendar);
            var userTimeZoneOffset = userTimeZone.GetUtcOffset(DateTime.UtcNow);
            var serverTimeOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

            var fromDateLocal = DateTimeOffset.Parse(dateFrom).Add(serverTimeOffset - userTimeZoneOffset);

            var dateUtc = fromDateLocal.ToUniversalTime();

            var fromDateUser = TimeZoneInfo.ConvertTime(fromDateLocal, userTimeZone);
            var fromDateCalendar = TimeZoneInfo.ConvertTime(fromDateUser, calendarTimeZone).Add(serverTimeOffset - userTimeZoneOffset);


            var dayNumberInUserTimeZone = fromDateUser.Day;
            var daysInMonthUserTimeZone = DateTime.DaysInMonth(fromDateUser.Year, fromDateUser.Month);

            var scheduleDate = fromDateCalendar;

            while (dayNumberInUserTimeZone <= daysInMonthUserTimeZone)
            {
                var weekDay = scheduleDate.DayOfWeek;

                var availability = scheduleList.FirstOrDefault(x => x.DayType == "weekday" && x.Value == weekDay.ToString());
                var availabilityCustom = scheduleList.FirstOrDefault(x => x.DayType == "date" && DateTime.Parse(x.Value) == scheduleDate.Date);

                if (availability != null || availabilityCustom != null)
                {
                    var dayStartFromInMinutes = 0d;
                    var dayEndFromINMinutes = 0d;

                    if (availabilityCustom != null)
                    {
                        dayStartFromInMinutes = availabilityCustom.From;
                        dayEndFromINMinutes = availabilityCustom.To;
                    }
                    else if (availability != null)
                    {
                        dayStartFromInMinutes = availability.From;
                        dayEndFromINMinutes = availability.To;
                    }
                    var timeSlots = GenerateTimeSlotForDate(scheduleDate, bufferTime, meetingDuration, dayStartFromInMinutes, dayEndFromINMinutes, userTimeZone);

                    timeSlots = timeSlots.Where(x => x.StartAt >=DateTime.Now );
                    timeSlots.ToList().ForEach(x => x.StartAt = TimeZoneInfo.ConvertTime(x.StartAt.DateTime, userTimeZone));

                    var eventAvailability = new EventTimeCalendar
                    {
                        Date = scheduleDate.ToString("yyyy-MMM-dd"),
                        Slots = timeSlots.ToList()
                    };
                    result.Add(eventAvailability);
                }

                scheduleDate = scheduleDate.AddDays(1);

                dayNumberInUserTimeZone++;
            }
            return result;
        }

        private IEnumerable<TimeSlot> GenerateTimeSlotForDate(
             DateTimeOffset calenderDate,
             double bufferTimeInMinute,
             int meetingDuration,
             double dayStartFromInMinutes,
             double dayEndInMinutes, TimeZoneInfo destnationTimeZone)
        {

            var dayStartTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayStartFromInMinutes);
            var dayEndTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayEndInMinutes);

            while (dayStartTime <= dayEndTime)
            {
                yield return new TimeSlot { StartAt = dayStartTime };
                dayStartTime = dayStartTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinute); ;

            }

        }
    }


}
