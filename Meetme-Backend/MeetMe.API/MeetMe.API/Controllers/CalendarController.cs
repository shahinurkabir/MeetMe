using MediatR;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Calendars.Commands;
using MeetMe.Application.EventTypes.Calendar;
using MeetMe.Application.EventTypes.Calendar.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator mediator;

        public CalendarController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("availability/{eventTypeId}")]
        public async Task<List<EventTimeCalendar>> CalendarAvailabilityByEventType(Guid eventTypeId, string timezone, string from, string to)
        {
            var command = new TimeSlotRangeQuery { EventTypeId = eventTypeId, TimeZone = timezone, FromDate = from, ToDate = to };

            var result = await mediator.Send(command);
            return result;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("appointment/new")]
        public async Task<Guid> AddNewAppointment(CreateAppointmentCommand createAppointmentCommand)
        {
            var result = await mediator.Send(createAppointmentCommand);
            return result;
        }

    }

}
