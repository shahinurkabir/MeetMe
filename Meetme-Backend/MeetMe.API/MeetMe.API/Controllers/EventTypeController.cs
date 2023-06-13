using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeetMe.Application.EventTypes.Create;
using MeetMe.Application.EventTypes.Manage;
using MeetMe.Application.EventTypes.Queries;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Application.EventTypes.Update;
using MeetMe.Core.Interface;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUserInfo loginUser;

        public EventTypesController(IMediator mediator, IUserInfo loginUser)
        {
            this.mediator = mediator;
            this.loginUser = loginUser;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<EventType>> GetList()
        {
            var eventTypeListQuery = new GetEventTypeListQuery { OwnerId = loginUser.Id };

            var result = await mediator.Send(eventTypeListQuery);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<EventType> GetDetails(Guid id)
        {
            var calendarDetailQuery = new GetEventTypeDetailQuery { EventTypeId = id }; ;

            var result = await mediator.Send(calendarDetailQuery);

            return result;
        }


        [HttpPost]
        [Route("")]
        public async Task<Guid> AddNew(CreateEventTypeCommand createEventTypeCommand)
        {
            var result = await mediator.Send(createEventTypeCommand);

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<bool> UpdateEventType(Guid id, UpdateInfoCommand updateInfoCommand)
        {
            updateInfoCommand.Id = id;

            var result = await mediator.Send(updateInfoCommand);

            return result;
        }

        [HttpGet]
        [Route("{id}/questions")]
        public async Task<List<EventTypeQuestion>> GetQuestionList(Guid id)
        {
            var result = await mediator.Send(new GetEventTypeQuestionsQuery { EventTypeId = id });
            return result;
        }


        [HttpPost]
        [Route("{id}/questions")]
        public async Task<bool> ConfugureQuestions(Guid id, UpdateQuestionCommand command)
        {
            command.EventTypeId = id;
            var result = await mediator.Send(command);

            return result;
        }

        [HttpGet]
        [Route("{id}/availability")]
        public async Task<List<EventTypeAvailabilityDetail>> GetEventAvailabilityDetail(Guid id)
        {
            var queryCommand = new GetEventTypeAvailabilityDetailQuery { EvnetTypeId = id };

            var result = await mediator.Send(queryCommand);

            return result;

        }

        [HttpPost]
        [Route("{id}/availability")]
        public async Task<bool> UpdateAvailability(Guid id, UpdateEventAvailabilityCommand updateAvailabilityCommand)
        {
            var result = await mediator.Send(updateAvailabilityCommand);

            return result;
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<bool> Activate(Guid id)
        {
            var command = new ActivateEventTypeCommand(id);

            var result = await mediator.Send(command);

            return result;
        }

        [HttpPut]
        [Route("{id}/deactivate")]
        public async Task<bool> Deactivate(Guid id)
        {
            var command = new DeactivateEventTypeCommand(id);

            var result = await mediator.Send(command);

            return result;
        }
    }
}
