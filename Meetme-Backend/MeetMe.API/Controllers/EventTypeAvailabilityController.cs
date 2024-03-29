﻿using MediatR;
using MeetMe.Application.EventTypes.Commands;
using MeetMe.Application.EventTypes.Queries;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypeAvailabilityController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventTypeAvailabilityController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<List<EventTypeAvailabilityDetail>> GetEventAvailabilityDetail(Guid id)
        {
            var queryCommand = new GetEventTypeAvailabilityQuery { EvnetTypeId = id };

            var result = await mediator.Send(queryCommand);

            return result;

        }

        [HttpPost]
        [Route("{id}")]
        public async Task<bool> UpdateAvailability(Guid id, UpdateEventAvailabilityCommand updateAvailabilityCommand)
        {
            var result = await mediator.Send(updateAvailabilityCommand);

            return result;
        }
    }
}
