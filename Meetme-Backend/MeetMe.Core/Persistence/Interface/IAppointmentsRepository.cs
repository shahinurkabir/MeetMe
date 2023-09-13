using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAppointmentsRepository
    {
        Task<Appointment> GetById(Guid id);
        Task AddAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Guid id);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsByDateRange(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC);
        Task<bool> IsTimeConflicting(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC);
    }
}
