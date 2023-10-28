using MediatR;
using MeetMe.Application.EventTypes.Commands;
using MeetMe.Application.EventTypes.Queries;
using MeetMe.Core.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypeQuestionController : ControllerBase
    {
        private readonly IMediator mediator;
        public EventTypeQuestionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<List<EventTypeQuestion>> GetQuestionList(Guid id)
        {
            var result = await mediator.Send(new GetEventTypeQuestionsQuery { EventTypeId = id });
            result = result.OrderBy(x => x.DisplayOrder).ToList();
            return result;
        }


        [HttpPost]
        [Route("{id}")]
        public async Task<bool> ConfigureQuestions(Guid id, UpdateQuestionCommand command)
        {
            command.EventTypeId = id;
            var result = await mediator.Send(command);

            return result;
        }
    }
}
