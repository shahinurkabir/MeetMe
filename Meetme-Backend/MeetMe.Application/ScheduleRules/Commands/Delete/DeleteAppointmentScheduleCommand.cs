using MediatR;
using System;

namespace MeetMe.Application.ScheduleRules.Commands.Delete
{
    public class DeleteAppointmentScheduleCommand : IRequest<Core.Dtos.ResponseDto>
    {
        public Guid AppointmentScheduleId { get; set; }
    }
}
