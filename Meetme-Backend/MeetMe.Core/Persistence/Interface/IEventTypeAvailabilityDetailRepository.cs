using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventTypeAvailabilityDetailRepository
    {
        Task<List<EventTypeAvailabilityDetail>> GetEventTypeAvailabilityDetailByEventId(Guid eventTypeId);
        Task RemoveItems(List<EventTypeAvailabilityDetail> itemsToRemove);
        Task InsertItems(List<EventTypeAvailabilityDetail> itemsToInsert);
    }
}
