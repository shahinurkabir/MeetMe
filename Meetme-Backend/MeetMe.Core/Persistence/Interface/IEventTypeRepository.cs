using System;
using MeetMe.Core.Persistence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventTypeRepository
    {
        Task AddNewEventType(EventType eventTypeInfo);
       // Task<bool> AddNewEventTypeScheduleRule(EventTypeScheduleInfo scheduleRule);

        Task<List<EventType>> GetEventTypeList();
        Task<List<EventType>> GetEventTypeListByUserId(Guid userId);
       // Task<bool> SlugUsedYN(string slug, Guid userId);
        Task<EventType> GetEventTypeById(Guid eventTypeId);
        //Task<EventTypeScheduleInfo> GetEventTypeScheduleById(Guid eventTypeId);

        //Task<List<EventTypeScheduleInfo>> GetScheduleList();

        //Task<bool> UpdateSchedule(EventTypeScheduleInfo scheduleRule);

        Task UpdateEventType(EventType eventTypeInfo);
        
    }

}