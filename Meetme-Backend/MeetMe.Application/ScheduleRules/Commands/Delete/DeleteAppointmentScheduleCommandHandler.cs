using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Dtos;

namespace MeetMe.Application.ScheduleRules.Commands.Delete
{
    public class DeleteAppointmentScheduleCommandHandler : IRequestHandler<DeleteAppointmentScheduleCommand, ResponseDto>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public DeleteAppointmentScheduleCommandHandler(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;
        }

        public async Task<ResponseDto> Handle(DeleteAppointmentScheduleCommand request, CancellationToken cancellationToken)
        {
            var model = await persistenseProvider.GetScheduleById(request.AppointmentScheduleId);

            if (model == null) throw new Exception("Invalid rule id provided");

            var result = await persistenseProvider.DeleteSchedule(request.AppointmentScheduleId);

            return new ResponseDto { Result=result};
        }
    }
}
