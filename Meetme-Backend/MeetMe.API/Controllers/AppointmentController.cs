using MediatR;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Calendars.Commands;
using MeetMe.Application.Calendars.Quaries;
using MeetMe.Core.Constants;
using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
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
            scheduleEventSearchParameters.OwnerId = loginUserInfo.Id;
            scheduleEventSearchParameters.TimeZone = loginUserInfo.TimeZone;

            if (string.IsNullOrWhiteSpace(scheduleEventSearchParameters.Period) ||
               scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Upcoming)
            {
                var (startDay, endDay) = DateTimeExtesions.GetUpcomingTimeByTimeZone(loginUserInfo.TimeZone);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Past)
            {
                var (startDay, endDay) = DateTimeExtesions.GetPastTimeByTimeZone(loginUserInfo.TimeZone);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.DateRange)
            {
                var (startDay, endDay) = DateTimeExtesions.GetDateRangeByTimeZone(loginUserInfo.TimeZone, scheduleEventSearchParameters.StartDate, scheduleEventSearchParameters.EndDate);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;

            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Today)
            {
                var (startDay, endDay) = DateTimeExtesions.GetTodayTimeByTimeZone(loginUserInfo.TimeZone);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.Tomorrow)
            {
                var (startDay, endDay) = DateTimeExtesions.GetTomorrowTimeByTimeZone(loginUserInfo.TimeZone);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.ThisWeek)
            {
                var (startDay, endDay) = DateTimeExtesions.GetThisWeekTimeByTimeZone(loginUserInfo.TimeZone);

                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
            }
            else if (scheduleEventSearchParameters.Period == Events.AppointmentSearchByDate.ThisMonth)
            {
                var (startDay, endDay) = DateTimeExtesions.GetThisMonthTimeByTimeZone(loginUserInfo.TimeZone);
                scheduleEventSearchParameters.StartDate = startDay;
                scheduleEventSearchParameters.EndDate = endDay;
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
