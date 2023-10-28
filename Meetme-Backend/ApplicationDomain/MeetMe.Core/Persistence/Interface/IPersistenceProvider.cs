using MeetMe.Core.Persistence.Entities;
using System.Reflection.Metadata;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IPersistenceProvider :IAvailabilityRepository, IEventTypeRepository, IEventQuestionRepository, IAppointmentRepository,IUserRepository
    {
        void EnsureDbCreated();
    }

}
