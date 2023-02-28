using MediatR;
using System;
using System.Collections.Generic;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Update
{
    public class UpdateAvailabilityCommand : IRequest<bool>
    {
        public UpdateAvailabilityCommand()
        {
            Details = new List<AvailabilityDetail>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TimeZoneId { get; set; }
        public List<AvailabilityDetail> Details { get; set; }
    }

}
