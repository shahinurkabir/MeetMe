using System;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommandResponse
    {
        public Guid Id { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public int ErrorCode { get; set; }
    }
}
