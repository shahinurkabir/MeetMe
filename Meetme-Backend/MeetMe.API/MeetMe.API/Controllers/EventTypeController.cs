using MediatR;
using Microsoft.AspNetCore.Mvc;
using MeetMe.Application.EventTypes.Queries;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Interface;
using MeetMe.Application.EventTypes.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.API.Models;
using MeetMe.Application.EventTypes.Commands.Update;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Application.EventTypes.Commands.Clone;
using MeetMe.Application.EventTypes.Commands.Manage;
using MeetMe.Application.EventTypes.Commands.Delete;
using MeetMe.Application.AccountSettings.Queries;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypeController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILoginUserInfo loginUser;
        private readonly IUserRepository userRepository;

        public EventTypeController(IMediator mediator, ILoginUserInfo loginUser, IUserRepository userRepository)
        {
            this.mediator = mediator;
            this.loginUser = loginUser;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Route("list")]
        public async Task<List<EventType>> GetList()
        {
            var eventTypeListQuery = new GetEventTypeListQuery { OwnerId = loginUser.Id };

            var result = await mediator.Send(eventTypeListQuery);

            return result;
        }


        //[AllowAnonymous]
        //[HttpGet]
        //[Route("calendar/{user_uri}")]
        //public async Task<AccountProfileDto?> GetUserCalendar(string user_uri)
        //{
        //    var response = await mediator.Send(new ProfileDetailQueryByBaseURI { BaseURI = user_uri });

        //    return response;
        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("{userURI}/list")]
        public async Task<UserProfileDetailResponse?> GetListByBaseURI(string userURI)
        {
            var userEnttiy = await userRepository.GetByBaseURI(userURI);

            if (userEnttiy == null) return null;

            var userProfileInfo = new AccountProfileDto
            {
                BaseURI = userURI,
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

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/calendar-availability")]
        public async Task<List<TimeSlotRangeDto>> AvailabilityByEventType(Guid id, string timezone, string from, string to)
        {
            var command = new TimeSlotRangeQuery { EventTypeId = id, TimeZone = timezone, FromDate = from, ToDate = to };

            var result = await mediator.Send(command);
            return result;

        }


    }
}
