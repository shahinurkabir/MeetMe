using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.Availabilities.Commands.Clone
{
    public class CloneAvailabilityCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

    }
}
