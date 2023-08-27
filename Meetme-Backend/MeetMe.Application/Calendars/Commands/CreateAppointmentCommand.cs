﻿using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using MeetMe.Core.Exceptions;
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

            DateTime startTimeUTC = request.StartTime.ToUniversalTime();
            DateTime endTimeUTC = startTimeUTC.AddMinutes(request.MeetingDuration);

            var isTimeConflicling = await appointmentsRepository.IsTimeConflicting(request.EventTypeId, startTimeUTC, endTimeUTC);

            if (isTimeConflicling == true)
            {
                throw new MeetMeException("The selected time slot is not available");
            }
            var newId = Guid.NewGuid();

            var entity = new Appointment
            {
                Id = newId,
                EventTypeId = request.EventTypeId,
                InviteeName = request.InviteeName,
                InviteeEmail = request.InviteeEmail,
                GuestEmails = request.GuestEmails,
                Status = AppointmentStatus.Active,
                StartTimeUTC =startTimeUTC,
                EndTimeUTC = endTimeUTC,
                Note = request.Note,
            };

            await appointmentsRepository.AddAppointment(entity);

            return newId;
        }
    }
    public class CreateAppointmentCommandValidation : AbstractValidator<CreateAppointmentCommand>
    {
    }

}
