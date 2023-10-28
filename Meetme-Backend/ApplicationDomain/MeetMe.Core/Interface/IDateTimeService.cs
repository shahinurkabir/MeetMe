using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Interface
{
    public interface IDateTimeService
    {
       // DateTime GetCurrentTime { get; }
        DateTime GetCurrentTimeUtc { get; }
    }
}
