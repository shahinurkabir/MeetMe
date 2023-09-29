using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Interface;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Availabilities.Commands;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILoginUserInfo applicationUserInfo;

        public AvailabilityController(IMediator mediator, ILoginUserInfo applicationUserInfo)
        {
            this.mediator = mediator;
            this.applicationUserInfo = applicationUserInfo;
        }

        [HttpGet]
        [Route("list")]
        public async Task<List<Availability>> GetList()
        {
            var queryCommand = new GetAvailabilityListQuery { UserId = applicationUserInfo.Id };

            var result = await mediator.Send(queryCommand);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Availability> GetDetail(Guid id)
        {
            var queryCommand = new GetAvailabilityDetailQuery { Id = id };

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
        public async Task<bool> EditName(Guid id, EditNameAvailabilityCommand editAvailabilityNameCommand)
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
        public async Task<bool> UpdateAvailability(Guid id, UpdateAvailabilityCommand updateAvailabilityCommand)
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

        [HttpPost]
        [Route("{id}/default")]
        public async Task<bool> SetDefault(Guid id, SetDefaultAvailabilityCommand  setDefaultAvailabilityCommand)
        {
            var result = await mediator.Send(setDefaultAvailabilityCommand);

            return result;
        }
    }
}
