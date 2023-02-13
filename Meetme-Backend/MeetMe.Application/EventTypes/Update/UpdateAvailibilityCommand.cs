using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Update
{
    public class UpdateAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int? ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public int TimeZoneId { get; set; }

        public List<AvailabilityDetailItem> AvailabilityDetails { get; set; } = null!;
    }

    public class AvailabilityDetailItem
    {
        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string Type { get; set; } = null!;
        public string? Day { get; set; }
        public DateTime? Date { get; set; }
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }

    }
    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateAvailabilityCommand, bool>
    {
        private readonly IEventAvailabilityRepository eventAvailabilityRepository;

        public UpdateAvailabilityCommandHandler(IEventAvailabilityRepository eventAvailabilityRepository)
        {
            this.eventAvailabilityRepository = eventAvailabilityRepository;
        }
        public async Task<bool> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var eventAvailability = ConvertToEntity(request);

            await eventAvailabilityRepository.ResetAvailability(eventAvailability);


            return await Task.FromResult(true);
        }

        private static EventTypeAvailability ConvertToEntity(UpdateAvailabilityCommand request)
        {

            var availability = new EventTypeAvailability
            {

                Id = request.Id,
                DateForwardKind = request.DateForwardKind,
                ForwardDuration = request.ForwardDuration,
                Duration = request.Duration,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                BufferTimeBefore = request.BufferTimeBefore,
                BufferTimeAfter = request.BufferTimeAfter,
                TimeZoneId = request.TimeZoneId,
                AvailabilityDetails = request.AvailabilityDetails.Select(e => new EventTypeAvailabilityDetail
                {
                    Id = Guid.NewGuid(),
                    AvailabilityId = request.Id,
                    Date = e.Date,
                    Day = e.Day,
                    From = e.From,
                    To = e.To,
                    StepId = e.StepId,
                    Type = e.Type
                }).ToList()
            };

            return availability;

        }
    }


}
