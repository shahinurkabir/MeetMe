using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetMe.Application.ScheduleRules.Queries;
using MeetMe.Application.ScheduleRules.Commands;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Interface;
using MeetMe.Application.ScheduleRules.Commands.Update;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleRuleController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUserInfo applicationUserInfo;

        public ScheduleRuleController(IMediator mediator, IUserInfo applicationUserInfo)
        {
            this.mediator = mediator;
            this.applicationUserInfo = applicationUserInfo;
        }

        [HttpGet]
        [Route("")]
        public async Task<List<ScheduleRule>> GetScheduleList()
        {
            var queryCommand = new ScheduleListQuery { UserId = applicationUserInfo.UserId };

            var result = await mediator.Send(queryCommand);

            return result;
        }

        [HttpGet]
        [Route("detail/{id}")]
        public async Task<ScheduleRule> GetDetail(Guid id)
        {
            var queryCommand = new ScheduleDetailQuery { Id = id };

            var result = await mediator.Send(queryCommand);

            return result;
        }

        [HttpPost]
        [Route("addnew")]
        public async Task<Guid> AddNewSchedule(CreateScheduleRuleCommand createRuleCommand)
        {
            var result = await mediator.Send(createRuleCommand);

            return result;
        }

        [HttpPost]
        [Route("update")]
        public async Task<bool> update(UpdateScheduleRuleCommand updateScheduleRuleCommand)
        {
            var result = await mediator.Send(updateScheduleRuleCommand);

            return result;
        }
    }
}
