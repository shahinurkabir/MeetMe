using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Constants
{
    public class Enums
    {
        public enum QuestionType
        {
            Text = 10,
            MultilineText = 15,
            PhoneNumber = 20,
            Compobox = 25,
            RadioButtons = 30,
            CheckBoxes = 35
        }
        public enum AppointmentSearchType
        {
            All = 0,
            Past = 1,
            OnGoing = 2,
            Upcoming = 3,
            DateRange = 4,
        }
        public enum AppointmentFilterFor
        {
            All = 0,
            EventType = 1,
            Status = 2,
            InviteeEmail = 3,
        }


    }
}
