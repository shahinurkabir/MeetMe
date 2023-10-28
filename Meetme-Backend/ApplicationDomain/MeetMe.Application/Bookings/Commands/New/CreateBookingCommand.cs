using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.Bookings.Commands.New
{
    public class CreateBookingCommand : IRequest<Guid>
    {
        public Guid EventTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string TimeZoneId { get; set; }
        public string GuestEmails { get; set; }
        public string Remarks { get; set; }

        //public InviteeInfo Invitee { get; set; }
        // public LocationInfo Location { get; set; }
        //public List<QuestionInfo> Questions { get; set; }
    }

    public class InviteeInfo
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string TimeZoneId { get; set; }
    }

    public class LocationInfo
    {
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string JoinURI { get; set; }

    }

    public class QuestionInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> AnswerOptions { get; set; }
        public bool Required { get; set; }
        public int Order { get; set; }
        public string Value { get; set; }
    }
}
