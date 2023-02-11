using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Exceptions
{
    public  class CustomException : Exception
    {
        public CustomException(string message) : base(message: message)
        {

        }
    }
}
