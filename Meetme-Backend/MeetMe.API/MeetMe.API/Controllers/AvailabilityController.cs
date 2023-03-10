using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Interface;
using MeetMe.Application.Availabilities.Commands.Create;
using MeetMe.Application.Availabilities.Commands.Update;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Availabilities.Commands.EditName;
using MeetMe.Application.Availabilities.Commands.Clone;
using MeetMe.Application.Availabilities.Commands.Delete;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUserInfo applicationUserInfo;

        public AvailabilityController(IMediator mediator, IUserInfo applicationUserInfo)
        {
            this.mediator = mediator;
            this.applicationUserInfo = applicationUserInfo;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<Availability>> GetList()
        {
            var queryCommand = new AvailabilityListQuery { UserId = applicationUserInfo.UserId };

            var result = await mediator.Send(queryCommand);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Availability> GetDetail(Guid id)
        {
            var queryCommand = new AvailabilityDetailQuery { Id = id };

            var result = await mediator.Send(queryCommand);

            return result;
        }

        [HttpPost]
        [Route("")]
        public async Task<Guid> AddNew(CreateAvailabilityCommand createAvailabilityCommand)
        {
            var result = await mediator.Send(createAvailabilityCommand);

            return result;
        }

        [HttpPost]
        [Route("{id}/EditName")]
        public async Task<bool> EditName(Guid id, EditAvailabilityNameCommand editAvailabilityNameCommand)
        {
            var result = await mediator.Send(editAvailabilityNameCommand);

            return result;
        }

        [HttpPost]
        [Route("{id}/clone")]
        public async Task<Guid> Clone(Guid id, CloneAvailabilityCommand  cloneAvailabilityCommand)
        {
            var result = await mediator.Send(cloneAvailabilityCommand);

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<bool> Edit(Guid id, UpdateAvailabilityCommand updateAvailabilityCommand)
        {
            updateAvailabilityCommand.Id = id;

            var result = await mediator.Send(updateAvailabilityCommand);

            return result;
        }
        [HttpPost]
        [Route("{id}/delete")]
        public async Task<bool> DeleteAvailability(Guid id, DeleteAvailabilityCommand  deleteAvailabilityCommand)
        {
            var result = await mediator.Send(deleteAvailabilityCommand);

            return result;
        }
    }
}
