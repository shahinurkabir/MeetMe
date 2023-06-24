using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Dtos;

namespace MeetMe.Application.Availabilities.Commands.Delete
{
    public class DeleteAvailabilityCommandHandler : IRequestHandler<DeleteAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public DeleteAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }

        public async Task<bool> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var entity = await availabilityRepository.GetById(request.Id);

            if (entity == null) throw new Exception("Invalid rule id provided");

            var result = await availabilityRepository.DeleteSchedule(entity);

            return true;
        }
    }
}
