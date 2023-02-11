using MeetMe.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentTime => DateTime.Now;

        public DateTime GetCurrentTimeUtc =>DateTime.UtcNow;
    }
}
