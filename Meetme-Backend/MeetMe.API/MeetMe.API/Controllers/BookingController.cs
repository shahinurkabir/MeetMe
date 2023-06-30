using MediatR;
using MeetMe.Application.Availabilities.Queries;
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
    public class BookingController : ControllerBase
    {
        private readonly IMediator mediator;

        public BookingController(IMediator mediator)
        {
            this.mediator = mediator;
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("calendar/event-type/{id}")]
        public async Task<List<EventTimeCalendar>> EventCalendar(Guid id, string timezone,string from, string to)
        {
            var command=new EventTimeCalendarCommand { EventTypeId=id, TimeZone=timezone, FromDate=from, ToDate=to };

            var result =await mediator.Send(command);
            return result;
            
        }

    }
}
