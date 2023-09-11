using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using MeetMe.Application.Services;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
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
    public class CancelAppointmentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public  string? CancellationReason { get; set; }

    }
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, bool>
    {
        private readonly IAppointmentsRepository appointmentsRepository;
        private readonly IDateTimeService dateTimeService;

        public CancelAppointmentCommandHandler(IAppointmentsRepository appointmentsRepository,IDateTimeService dateTimeService)
        {
            this.appointmentsRepository = appointmentsRepository;
            this.dateTimeService = dateTimeService;
        }
        public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentEntity = await appointmentsRepository.GetById(request.Id);

            if (appointmentEntity == null)
            {
                throw new MeetMeException("The appointment does not exist");
            }

            appointmentEntity.CancellationReason =request.CancellationReason;
            appointmentEntity.Status = AppointmentStatus.Cancelled; 
            appointmentEntity.DateCancelled = dateTimeService.GetCurrentTimeUtc;
            
            return await appointmentsRepository.UpdateAppointment(appointmentEntity);

        }
    }
    public class CancelAppointmentCommandValidation : AbstractValidator<CancelAppointmentCommand>
    {
    }

}
