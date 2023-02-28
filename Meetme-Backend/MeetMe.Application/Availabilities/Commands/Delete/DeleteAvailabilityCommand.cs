using MediatR;
using System;

namespace MeetMe.Application.Availabilities.Commands.Delete
{
    public class DeleteAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
