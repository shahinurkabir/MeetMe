﻿using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IAppointmentsRepository
    {
        Task<Appointment> GetAppointment(Guid id);
        Task AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(Appointment appointment);
        Task<bool> DeleteAppointment(Guid id);
        Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id);
        Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC);
        Task<List<AppointmentDetailsDto>> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC);
        Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId);
    }
}
