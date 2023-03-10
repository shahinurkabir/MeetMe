using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Constant
{
    public class Enums
    {
        public enum DateForwardKind
        {
            Moving = 10,
            DateRange = 20,
            Forever = 30
        }

        public enum QuestionType
        {
            Text = 10,
            MultilineText = 15,
            PhoneNumber = 20,
            Compobox = 25,
            RadioButtons = 30,
            CheckBoxes = 35
        }

    }
}
