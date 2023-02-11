using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.ScheduleRules.Commands.Clone
{
    public class CloneAppointmentScheduleCommand : IRequest<Guid>
    {
        public Guid AppointmentScheduleId { get; set; }

    }
}
