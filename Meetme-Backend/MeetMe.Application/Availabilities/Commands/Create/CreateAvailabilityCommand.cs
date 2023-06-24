using MediatR;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public string timeZone { get; set; } = null!;

    }
    public class AvailabilityDetailDto
    {

        public string DayType { get; set; } = null!;
        public string? Value { get; set; }
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}
