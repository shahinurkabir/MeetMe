using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using System.Collections.Generic;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Schedule
{
    public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, bool>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public UpdateScheduleCommandHandler(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;
        }

        public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var scheduleDetail = CommandToModel(request);

            var result = await persistenseProvider.UpdateSchedule(scheduleDetail);

            return await Task.FromResult(result);
        }

        private static EventTypeScheduleInfo CommandToModel(UpdateScheduleCommand request)
        {
            var scheduleDetails = new EventTypeScheduleInfo
            {
                //CalendarId = request.CalendarId,
                //TimeZoneId = request.TimeZoneId,
                //DateForwardKind = request.DateForwardKind,
                //ForwardDuration = request.ForwardDuration,
                //Duration = request.Duration,
                //BufferTimeAfter = request.BufferTimeAfter,
                //BufferTimeBefore = request.BufferTimeBefore,
                //DateFrom = request.DateFrom,
                //DateTo = request.DateTo,
                WeeklyTimeSchedule = CopyTimeSlotFromCommand(request)
            };

            return scheduleDetails;
        }

        private static List<DailyTimeSchedule> CopyTimeSlotFromCommand(UpdateScheduleCommand request)
        {
            var listTimeSlots = new List<DailyTimeSchedule>();

            request.WeeklyTimeSlots.ForEach(e =>
            {
                var item = new DailyTimeSchedule
                {
                    Type = e.Type,
                    Day = e.Day,
                    Date = e.Date,
                    From = e.From,
                    To = e.To
                };

                listTimeSlots.Add(item);
            });

            return listTimeSlots;
        }
    }
}
