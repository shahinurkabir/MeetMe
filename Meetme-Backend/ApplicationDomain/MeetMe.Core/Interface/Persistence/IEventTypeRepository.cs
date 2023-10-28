using System;
using MeetMe.Core.Persistence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetMe.Core.Interface.Persistence
{
    public interface IEventTypeRepository
    {
        Task<bool> AddNewEventType(EventTypeInfo eventTypeInfo);
        Task<bool> AddNewEventTypeScheduleRule(EventTypeScheduleInfo scheduleRule);

        Task<List<EventTypeInfo>> GetEventTypeList();
        Task<EventTypeInfo> GetEventTypeById(Guid eventTypeId);
        Task<EventTypeScheduleInfo> GetEventTypeScheduleById(Guid eventTypeId);

        Task<List<EventTypeScheduleInfo>> GetScheduleList();

        Task<bool> UpdateSchedule(EventTypeScheduleInfo scheduleRule);

        Task<bool> UpdateEventType(EventTypeInfo eventTypeInfo);

    }
}