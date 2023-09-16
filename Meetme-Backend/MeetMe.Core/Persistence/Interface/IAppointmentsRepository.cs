using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAppointmentsRepository
    {
        Task<Appointment> GetById(Guid id);
        Task AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Guid id);
        Task<AppointmentDetailsDto?> GetDetailsById(Guid id);
        Task<bool> IsTimeBooked(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC);
        Task<List<AppointmentDetailsDto>> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC);
        Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId);
    }
}
