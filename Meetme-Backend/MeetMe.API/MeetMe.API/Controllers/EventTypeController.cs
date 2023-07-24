using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeetMe.Application.EventTypes.Manage;
using MeetMe.Application.EventTypes.Queries;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Application.EventTypes.Update;
using MeetMe.Core.Interface;
using MeetMe.Application.EventTypes.Commands.Create;
using MeetMe.Application.EventTypes.Calendar.Dtos;
using MeetMe.Application.EventTypes.Calendar;
using Microsoft.AspNetCore.Authorization;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.API.Models;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILoginUserInfo loginUser;
        private readonly IUserRepository userRepository;

        public EventTypesController(IMediator mediator, ILoginUserInfo loginUser, IUserRepository userRepository)
        {
            this.mediator = mediator;
            this.loginUser = loginUser;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<EventType>> GetList()
        {
            var eventTypeListQuery = new GetEventTypeListQuery { OwnerId = loginUser.Id };

            var result = await mediator.Send(eventTypeListQuery);

            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("list/{baseURI}")]
        public async Task<UserProfileDetailResponse?> GetListByBaseURI(string baseURI)
        {
            var userEnttiy = await userRepository.GetByBaseURI(baseURI);

            if (userEnttiy == null) return null;

            var userProfileInfo = new AccountProfileDto
            {
                BaseURI = baseURI,
                UserName = userEnttiy.UserName,
                TimeZone = userEnttiy.TimeZone,
                WelcomeText = userEnttiy.WelcomeText
            };

            var eventTypeListQuery = new GetEventTypeListQuery { OwnerId = userEnttiy.Id };

            var commandResult = await mediator.Send(eventTypeListQuery);
            var result = new UserProfileDetailResponse
            {
                Profile = userProfileInfo,
                Events = commandResult
            };

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
            result = result.OrderBy(x => x.DisplayOrder).ToList();
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
            var queryCommand = new GetEventTypeAvailabilityQuery { EvnetTypeId = id };

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
        [Route("{id}/toggle-status")]
        public async Task<bool> ToggleStatus(Guid id)
        {
            var command = new ToggleEventTypeStatusCommand(id);

            var result = await mediator.Send(command);

            return result;
        }

        [HttpPut]
        [Route("{id}/clone")]
        public async Task<Guid> Clone(Guid id)
        {
            var command = new CloneEventTypeCommand(id);

            var result = await mediator.Send(command);

            return result;
        }

        [HttpPut]
        [Route("{id}/delete")]
        public async Task<bool> Delete(Guid id)
        {
            var command = new DeleteEventTypeCommand(id);

            var result = await mediator.Send(command);

            return result;
        }

       
    }
}
