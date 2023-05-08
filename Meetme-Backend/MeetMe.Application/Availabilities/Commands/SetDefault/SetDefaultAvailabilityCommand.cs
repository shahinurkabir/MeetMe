using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Availabilities.Commands.SetDefault
{
    public class SetDefaultAvailabilityCommand:IRequest<bool>
    {
        public Guid Id  { get; set; }
    }

}
