using MediatR;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Core.Constants;
using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System.ComponentModel.DataAnnotations;

namespace MeetMe.Application.EventTypes.Queries
{
    public class TimeSlotRangeQuery : IRequest<List<TimeSlotRangeDto>>
    {
        public Guid EventTypeId { get; set; }
        public string TimeZone { get; set; } = null!;
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
    }

    public class TimeSlotRangeQueryHandler : IRequestHandler<TimeSlotRangeQuery, List<TimeSlotRangeDto>>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IEventTypeAvailabilityRepository eventTypeAvailabilityDetailRepository;
        private readonly IAppointmentsRepository appointmentsRepository;

        public TimeSlotRangeQueryHandler
        (
            IEventTypeRepository eventTypeRepository,
            IEventTypeAvailabilityRepository eventTypeAvailabilityDetailRepository,
            IAppointmentsRepository appointmentsRepository
        )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task<List<TimeSlotRangeDto>> Handle(TimeSlotRangeQuery request, CancellationToken cancellationToken)
        {
            var tempFromDate = DateTime.Parse(request.FromDate);
            var tempToDate = DateTime.Parse(request.ToDate);

            var tempFromUTC = tempFromDate.ToUniversalTime();
            var tempToUTC = tempToDate.ToUniversalTime();

            var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.EventTypeId);
            var scheduleDetailEntityList = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityByEventId(request.EventTypeId);
            var appointmentList = await appointmentsRepository.GetAppointmentsOfEventTypeByDateRange(request.EventTypeId, tempFromUTC, tempToUTC);

            var bufferTime = eventTypeEntity.BufferTimeAfter;
            var meetingDuration = eventTypeEntity.Duration;

            var timeZoneInfo_User = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);
            var timeZoneInfo_Calendar = TimeZoneInfo.FindSystemTimeZoneById(eventTypeEntity.TimeZone);


            var dateFrom_User = TimeZoneInfo.ConvertTime(tempFromDate, timeZoneInfo_User);
            var dateTo_User = TimeZoneInfo.ConvertTime(tempToDate, timeZoneInfo_User);

            //add 1 day before and after to include all the slots
            var dateFrom_Calendar = TimeZoneInfo.ConvertTime(dateFrom_User, timeZoneInfo_Calendar).AddDays(-1);
            var dateTo_Calendar = TimeZoneInfo.ConvertTime(dateTo_User, timeZoneInfo_Calendar).AddDays(1);


            var listTimeSlots = GenerateTimeSlots(dateFrom_Calendar, dateTo_Calendar, meetingDuration, bufferTime, scheduleDetailEntityList);


            List<TimeSlotRangeDto> result = FinalizeScheduleDates(timeZoneInfo_User, meetingDuration, appointmentList, ref listTimeSlots);

            return result;

        }

        private static List<TimeSlotRangeDto> FinalizeScheduleDates(TimeZoneInfo timeZoneInfo_User, int meetingDuration, List<AppointmentDetailsDto> appointmentsBooked, ref List<TimeSlot> listTimeSlots)
        {
            //exclude slots that are already passed
            listTimeSlots = listTimeSlots.Where(e => e.StartDateTime > DateTimeOffset.UtcNow).ToList();

            //remove slots that are already booked
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

        private List<TimeSlot> GenerateTimeSlots
        (
            DateTime dateFrom_Calendar,
            DateTime dateTo_Calendar,
            int meetingDuration,
            int bufferTime,
            List<EventTypeAvailabilityDetail> availabilityDetails
        )
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
                        double dayStartFromInMinutes = scheduleItem.From;
                        double dayEndFromInMinutes = scheduleItem.To;

                        var timeSlots = GenerateTimeSlotForDate(dateRunning, bufferTime, meetingDuration, dayStartFromInMinutes, dayEndFromInMinutes).ToList();

                        result.AddRange(timeSlots);
                    }
                }

                dateRunning = dateRunning.AddDays(1);

            }
            return result;
        }

        private static List<EventTypeAvailabilityDetail> GetSchedulesForDate
        (
            List<EventTypeAvailabilityDetail> availabilityDetails,
            DateTime scheduleDate, DayOfWeek weekDay
        )
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

        private static IEnumerable<TimeSlot> GenerateTimeSlotForDate
        (
             DateTimeOffset calenderDate,
             double bufferTimeInMinute,
             int meetingDuration,
             double dayStartFromInMinutes,
             double dayEndInMinutes
         )
        {
            var dayStartTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayStartFromInMinutes);
            var dayEndTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayEndInMinutes);

            while (dayStartTime < dayEndTime)
            {
                yield return new TimeSlot { StartDateTime = dayStartTime.ToUniversalTime() };

                dayStartTime = dayStartTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinute); ;
            }

        }
    }


}
