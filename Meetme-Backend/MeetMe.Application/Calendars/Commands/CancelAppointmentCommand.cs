using FluentValidation;
using FluentValidation.Validators;
using MediatR;
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
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IDateTimeService _dateTimeService;

        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentsRepository,IDateTimeService dateTimeService)
        {
            this._appointmentsRepository = appointmentsRepository;
            this._dateTimeService = dateTimeService;
        }
        public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointmentEntity = await _appointmentsRepository.GetAppointment(request.Id);

            if (appointmentEntity == null)
            {
                throw new MeetMeException("The appointment does not exist");
            }

            appointmentEntity.CancellationReason =request.CancellationReason;
            appointmentEntity.Status = AppointmentStatus.Cancelled; 
            appointmentEntity.DateCancelled = _dateTimeService.GetCurrentTimeUtc;
            
            return await _appointmentsRepository.UpdateAppointment(appointmentEntity);

        }
    }
    public class CancelAppointmentCommandValidation : AbstractValidator<CancelAppointmentCommand>
    {
    }

}
