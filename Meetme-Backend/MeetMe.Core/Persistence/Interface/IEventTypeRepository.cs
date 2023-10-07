using System;
using MeetMe.Core.Persistence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventTypeRepository
    {
        Task<bool> AddNewEventType(EventType eventTypeInfo);
        Task<List<EventType>?> GetEventTypeListByUserId(Guid userId);
        Task<EventType?> GetEventTypeById(Guid eventTypeId);
        Task<bool> UpdateEventType(EventType eventTypeInfo);
        Task<bool> UpdateEventAvailability(EventType eventTypeInfo);
        Task<EventType?> GetEventTypeBySlug(string slug);
    }
}