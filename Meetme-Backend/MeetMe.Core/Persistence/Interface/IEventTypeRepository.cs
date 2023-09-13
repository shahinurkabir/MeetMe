using System;
using MeetMe.Core.Persistence.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventTypeRepository
    {
        Task AddNewEventType(EventType eventTypeInfo);
        Task<List<EventType>> GetEventTypeListByUserId(Guid userId);
        Task<EventType> GetEventTypeById(Guid eventTypeId);
        Task UpdateEventType(EventType eventTypeInfo);
        Task UpdateEventAvailability(EventType eventTypeInfo, List<EventTypeAvailabilityDetail> eventTypeAvailabilityDetails);
        Task<EventType> GetEventTypeBySlug(string slug);
    }



}