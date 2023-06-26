using MediatR;
using MeetMe.Application.EventTypes.Calendar.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System.ComponentModel.DataAnnotations;

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

            var availabilityList = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityDetailByEventId(eventTypeEntity.Id);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);
            var listSlots = GetCalendarTimeSlots(request.TimeZone, calendarTimeZone, request.FromDate, request.ToDate, bufferTime, meetingDuration, availabilityList);

           // var listSlots = eventTimeCalendarDetails.SelectMany(e => e.Slots).ToList();

            listSlots.ForEach(e => e.StartAt = TimeZoneInfo.ConvertTime(e.StartAt, userTimeZone));

            var listDatesGroup = listSlots.GroupBy(e => e.StartAt.DateTime.Date);

            var result = new List<EventTimeCalendar>();

            foreach (var dateGroup in listDatesGroup)
            {
                var date = dateGroup.Key;
                var listDate = dateGroup.ToList();
                listDate = listDate.Where(e => e.StartAt.DateTime.Date == date).ToList();

                var eventTimeCalendar = new EventTimeCalendar
                {
                    Date = date.ToString("yyyy-MMM-dd"),  
                    Slots = listDate
                };
                result.Add(eventTimeCalendar);
            }
            return result;

        }

        private List<TimeSlot> GetCalendarTimeSlots(
            string timezoneUser, string timezoneCalendar,
            string dateFrom, string dateTo, int bufferTime,
            int meetingDuration, List<EventTypeAvailabilityDetail> scheduleList
            )
        {
            var result = new List<TimeSlot>();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneUser);
            var calendarTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneCalendar);
            
            var tempFromDate = DateTime.Parse(dateFrom);
            var tempToDate = DateTime.Parse(dateTo);

            var tempFromUTC = tempFromDate.ToUniversalTime();
            var tempToUTC = tempToDate.ToUniversalTime();

            var dateFromUser = TimeZoneInfo.ConvertTime(tempFromDate, userTimeZone);
            var dateToUser = TimeZoneInfo.ConvertTime(tempToDate, userTimeZone);

            var dateFromCalendar = TimeZoneInfo.ConvertTime(dateFromUser, calendarTimeZone).AddDays(-1);
            var dateToCalendar = TimeZoneInfo.ConvertTime(dateToUser, calendarTimeZone).AddDays(1);

            var scheduleDate = dateFromCalendar;

            while (scheduleDate <= dateToCalendar)
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
                    var timeSlots = GenerateTimeSlotForDate(scheduleDate, bufferTime, meetingDuration, dayStartFromInMinutes, dayEndFromINMinutes, userTimeZone).ToList();

                    //var eventAvailability = new EventTimeCalendar
                    //{
                    //    Date = scheduleDate.ToString("yyyy-MMM-dd"),
                    //    Slots = timeSlots.ToList()
                    //};
                    result.AddRange(timeSlots);
                }

                scheduleDate = scheduleDate.AddDays(1);

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

            while (dayStartTime < dayEndTime)
            {
                yield return new TimeSlot { StartAt = dayStartTime };
                dayStartTime = dayStartTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinute); ;

            }

        }
    }


}
