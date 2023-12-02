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

        [HttpGet]
        [Route("schedule-event")]
        public async Task<AppointmentsPaginationResult> ScheduleEvents([FromQuery] AppointmentSearchParametersDto scheduleEventSearchParameters)
        {
            var loginUserId = loginUserInfo.Id;
            scheduleEventSearchParameters.OwnerId = loginUserInfo.Id;
            scheduleEventSearchParameters.TimeZone = loginUserInfo.TimeZone;

            var currentTimeUtc = DateTime.UtcNow;

          
            if (string.IsNullOrWhiteSpace(scheduleEventSearchParameters.Period) ||
               scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Upcoming)
            {
                scheduleEventSearchParameters.StartDate = currentTimeUtc.AddMinutes(1);
                scheduleEventSearchParameters.EndDate = DateTime.MaxValue;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Past)
            {
                scheduleEventSearchParameters.StartDate = DateTime.MinValue;
                scheduleEventSearchParameters.EndDate = currentTimeUtc;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.DateRange)
            {
                scheduleEventSearchParameters.StartDate = scheduleEventSearchParameters.StartDate?.ToUniversalTime();
                scheduleEventSearchParameters.EndDate = scheduleEventSearchParameters.EndDate?.ToUniversalTime();

                // both date are same then add one day to end date
                if (scheduleEventSearchParameters.StartDate.HasValue && scheduleEventSearchParameters.EndDate.HasValue &&
                                       scheduleEventSearchParameters.StartDate.Value.Date == scheduleEventSearchParameters.EndDate.Value.Date)
                {
                    scheduleEventSearchParameters.EndDate = scheduleEventSearchParameters.EndDate.Value.AddDays(1);
                }
                
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Today)
            {
                scheduleEventSearchParameters.StartDate = currentTimeUtc;
                scheduleEventSearchParameters.EndDate = currentTimeUtc.AddDays(1);
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Tomorrow)
            {
                scheduleEventSearchParameters.StartDate = currentTimeUtc.AddDays(1);
                scheduleEventSearchParameters.EndDate = currentTimeUtc.AddDays(2);
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.ThisWeek)
            {
                scheduleEventSearchParameters.StartDate = currentTimeUtc.AddDays(-(int)currentTimeUtc.DayOfWeek);
                scheduleEventSearchParameters.EndDate = currentTimeUtc.AddDays(-(int)currentTimeUtc.DayOfWeek + 7);
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.ThisMonth)
            {
                scheduleEventSearchParameters.StartDate = new DateTime(currentTimeUtc.Year, currentTimeUtc.Month, 1);
                scheduleEventSearchParameters.EndDate = new DateTime(currentTimeUtc.Year, currentTimeUtc.Month, 1).AddMonths(1);
            }
            var command = new AppointmentListQuery
            {
                SearchParameters = scheduleEventSearchParameters,
                PageNumber = scheduleEventSearchParameters.PageNumber,
                PageSize = Events.DefaulData.Pagination_PageSize,
            };

            var result = await mediator.Send(command);

            return result;
        }

    }



}
