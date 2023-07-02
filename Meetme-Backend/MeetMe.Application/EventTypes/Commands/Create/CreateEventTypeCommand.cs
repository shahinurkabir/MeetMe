using MediatR;
using System;
using MeetMe.Core.Constant;

namespace MeetMe.Application.EventTypes.Commands.Create
{
    public class CreateEventTypeCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string Slug { get; set; } = null!;

        public string EventColor { get; set; } = null!;
        public bool ActiveYN { get; set; }
        public string TimeZoneName { get; set; } = null!;
        

        public CreateEventTypeCommand()
        {
        }

    }
}
