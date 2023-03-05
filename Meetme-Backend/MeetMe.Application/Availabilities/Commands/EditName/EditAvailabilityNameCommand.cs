using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class EditAvailabilityNameCommand:IRequest<bool>
    {
        public Guid Id  { get; set; }
        public string Name { get; set; } = null!;
    }

}
