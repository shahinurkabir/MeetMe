using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventAvailabilityRepository
    {
        Task<EventTypeAvailability> GetEventAvailabilityById(Guid eventTypeId);

        Task ResetAvailability(EventTypeAvailability eventTypeAvailability);

        //Task AddNewEventAvailability(EventTypeAvailability eventTypeAvailability);
        
        //Task UpdateEventAvailability(EventTypeAvailability eventTypeAvailability);
        //Task DeleteEventAvailability(Guid eventTypeId);
    }

}