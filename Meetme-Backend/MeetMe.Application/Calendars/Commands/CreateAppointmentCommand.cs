using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MeetMe.Core.Constants.Events;

namespace MeetMe.Application.Calendars.Commands
{
    public class CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid EventTypeId { get; set; }
        public string InviteeName { get; set; } = null!;
        public string InviteeEmail { get; set; } = null!;
        public string? GuestEmails { get; set; }
        public DateTime StartTime { get; set; }
        public int MeetingDuration { get; set; }
        public string? Note { get; set; }

    }
    public class CreateAppoimentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentsRepository appointmentsRepository;

        public CreateAppoimentCommandHandler(IAppointmentsRepository appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }
        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            var startTimeUTC=request.StartTime.ToUniversalTime();
            var endTimeUTC = startTimeUTC.AddMinutes(request.MeetingDuration);
            
            var entity = new CalendarAppointment
            {
                Id = newId,
                EventTypeId = request.EventTypeId,
                InviteeName = request.InviteeName,
                InviteeEmail = request.InviteeEmail,
                GuestEmails = request.GuestEmails,
                Status = AppointmentStatus.Active,
                StartTime =startTimeUTC,
                EndTime = endTimeUTC,
                Note = request.Note,
            };

            await appointmentsRepository.AddNewAppointment(entity);

            return newId;
        }
    }
    public class CreateAppointmentCommandValidation : AbstractValidator<CreateAppointmentCommand>
    {
    }

}
