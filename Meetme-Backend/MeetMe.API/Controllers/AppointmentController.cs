using MediatR;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Calendars.Commands;
using MeetMe.Application.Calendars.Quaries;
using MeetMe.Core.Constants;
using MeetMe.Core.Dtos;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo loginUserInfo;

        public AppointmentController(IMediator mediator, IPersistenceProvider persistenceProvider, ILoginUserInfo loginUserInfo)
        {
            this.mediator = mediator;
            this.persistenceProvider = persistenceProvider;
            this.loginUserInfo = loginUserInfo;
        }

        [HttpGet]
        [Route("me")]
        public async Task<List<AppointmentDetailsDto>?> GetMyAppointmentList()
        {
            var loginUserId = loginUserInfo.Id;
            var result = await persistenceProvider.GetAppointmentListByUser(loginUserId);
            return result;
        }

        [HttpGet]
        [Route("eventtype/{id}")]
        public async Task<AppointmentDetailsDto> GetListByEventTypeId(Guid id)
        {
            var result = await mediator.Send(new AppointmentDetailsQuery { Id = id });
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/details")]
        public async Task<AppointmentDetailsDto> GetDetails(Guid id)
        {
            var result = await mediator.Send(new AppointmentDetailsQuery { Id = id });
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("new")]
        public async Task<Guid> AddNewAppointment(CreateAppointmentCommand createAppointmentCommand)
        {
            var result = await mediator.Send(createAppointmentCommand);
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{id}/cancel")]
        public async Task<bool> CancelAppointment(CancelAppointmentCommand cancelAppointmentCommand)
        {
            var result = await mediator.Send(cancelAppointmentCommand);
            return result;
        }

        [HttpPost]
        [Route("schedule-event/{pageNumber}")]
        public async Task<AppointmentsPaginationResult> ScheduleEvents( int pageNumber, AppointmentSearchParametersDto scheduleEventSearchParameters)
        {
            var loginUserId = loginUserInfo.Id;
            scheduleEventSearchParameters.OwnerId = loginUserInfo.Id; 

            var currentTimeUtc = DateTime.UtcNow;

            if (scheduleEventSearchParameters.SearchBy==Events.AppointmentSearchBy.Ongoing)
            {
                scheduleEventSearchParameters.StartDate = currentTimeUtc;
                scheduleEventSearchParameters.EndDate = currentTimeUtc.AddHours(1);
            }
            else if (scheduleEventSearchParameters.SearchBy == Events.AppointmentSearchBy.Upcoming)
            {
                scheduleEventSearchParameters.StartDate= currentTimeUtc;
                scheduleEventSearchParameters.EndDate= DateTime.MaxValue;
            }
            else if (scheduleEventSearchParameters.SearchBy == Events.AppointmentSearchBy.Past)
            {
                scheduleEventSearchParameters.StartDate = DateTime.MinValue;
                scheduleEventSearchParameters.EndDate = currentTimeUtc;
            }
            else if (scheduleEventSearchParameters.SearchBy == Events.AppointmentSearchBy.DateRange)
            {
                scheduleEventSearchParameters.StartDate = scheduleEventSearchParameters.StartDate?.ToUniversalTime();
                scheduleEventSearchParameters.EndDate = scheduleEventSearchParameters.EndDate?.ToUniversalTime();
            }
            var command=new AppointmentListQuery
            {
                SearchParameters = scheduleEventSearchParameters,
                PageNumber = pageNumber,
                PageSize=Events.DefaulData.Pagination_PageSize
            };

            var result = await mediator.Send(command);

            return result;
        }

    }

    

}
