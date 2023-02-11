using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IPersistenceProvider :  IBookingRepository, IScheduleRuleRepository
    {
        
    }

}
