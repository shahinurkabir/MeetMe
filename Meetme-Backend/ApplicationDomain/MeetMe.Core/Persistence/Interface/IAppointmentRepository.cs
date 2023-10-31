using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetAppointment(Guid id);
        Task<bool> AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Guid id);
        Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id);
        Task<bool> IsTimeBooked(Guid eventTypeId, DateTime formDateTimeUTC, DateTime toDateTimeUTC);
        Task<List<AppointmentDetailsDto>> GetAppointmentListByEventType(Guid eventTypeId, DateTime formDateTimeUTC, DateTime toDateTimeUTC);
        Task<List<AppointmentDetailsDto>> GetAppointmentListByUser(Guid userId);
        Task<(int, List<AppointmentDetailsDto>?)> GetAppintmentListByParameters(AppointmentSearchParametersDto searchParametersDto, int pageNumber, int pageSize);
    }
}
