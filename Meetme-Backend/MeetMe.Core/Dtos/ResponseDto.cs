using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Dtos
{
    public class ResponseDto
    {
        public Guid Id { get; set; }

        public bool Result { get; set; }

        public string Message { get; set; }
        public string ErrorMessage { get; set; }
    }
}
