using MediatR;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Application.Calendars.Commands;
using MeetMe.Application.Calendars.Quaries;
using MeetMe.Application.Calendars.Quaries.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator mediator;

        public AppointmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("{id}/details")]
        public async Task<AppointmentDetailsDto> GetDetails(Guid id)
        {
            var result = await mediator.Send(new AppointmentDetailsQuery { Id=id});
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
        [HttpGet]
        [Route("{id}/cancel")]
        public async Task<bool> CancelAppointment(CancelAppointmentCommand cancelAppointmentCommand)
        {
            var result = await mediator.Send(cancelAppointmentCommand);
            return result;
        }

    }

}
