using MediatR;
using System;
using MeetMe.Core.Constant;

namespace MeetMe.Application.EventTypes.Create
{
    public class CreateEventTypeCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;

        public string? Descripton { get; set; } 

        public string? Location { get; set; }

        public string Slug { get; set; }=null!;

        public string EventColor { get; set; }=null!;
        public bool ActiveYN { get; set; }
        public string TimeZoneName { get; set; } = null!;
        /// <summary>
        /// M : Moving 
        //  D : DateRange
        //  F : Foreever
        /// </summary>
        //public string DateForwardKind { get; set; }
        //public int? ForwardDuration { get; set; }
        //public DateTime? DateFrom { get; set; }
        //public DateTime? DateTo { get; set; }
        //public int Duration { get; set; }
        //public int BufferTimeBefore { get; set; }
        //public int BufferTimeAfter { get; set; }
        //public int TimeZoneId { get; set; }
        //public Guid? BaseScheduleId { get; set; }

        public CreateEventTypeCommand()
        {
            //DateForwardKind = Constants.Events.ForwandDateKInd.Moving;
            //ForwardDuration = Constants.Events.ForwardDuration;
            //Duration = Constants.Events.MeetingDuration;
            //BufferTimeBefore = Constants.Events.BufferTimeDuration;
            //BufferTimeAfter = Constants.Events.BufferTimeDuration;
        }

    }
}
