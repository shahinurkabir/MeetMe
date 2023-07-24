using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAppointmentsRepository
    {
        Task<CalendarAppointment> GetById(Guid id);
        Task AddNewAppointment(CalendarAppointment appointment);
    }
}
