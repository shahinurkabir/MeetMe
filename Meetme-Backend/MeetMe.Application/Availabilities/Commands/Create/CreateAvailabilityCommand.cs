using MediatR;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommand : IRequest<Guid>
    {
        public CreateAvailabilityCommand()
        {
            Details = new List<AvailabilityDetail>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TimeZoneId { get; set; }
        public List<AvailabilityDetail> Details { get; set; }
    }

}
