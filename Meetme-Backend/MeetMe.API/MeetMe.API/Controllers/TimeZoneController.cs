using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ITimeZoneDataRepository timeZoneDataRepository;

        public TimeZoneController(IMediator mediator, ITimeZoneDataRepository timeZoneDataRepository)
        {
            this.mediator = mediator;
            this.timeZoneDataRepository = timeZoneDataRepository;
        }
        [HttpGet]
        [Route("")]
        public async Task<List<TimeZoneData>> GetList()
        {
            var result = await timeZoneDataRepository.GetTimeZoneList();
            return result;
        }
        [HttpGet]
        [Route("{name}")]
        public async Task<TimeZoneData?> GetByName(string name)
        {
            var result = await timeZoneDataRepository.GetTimeZoneByName(name);
            return result;
        }
    }
}
