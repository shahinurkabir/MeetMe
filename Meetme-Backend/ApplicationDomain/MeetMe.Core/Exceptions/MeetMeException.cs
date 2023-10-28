using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Exceptions
{
    public  class MeetMeException : Exception
    {
        public MeetMeException(string message) : base(message: message)
        {

        }
    }
}
