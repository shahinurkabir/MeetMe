using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Interface.Persistence
{
    public interface IPersistenseProvider : IEventTypeRepository, IBookingRepository, IAvailabilityRuleRepository
    {
    }
}
