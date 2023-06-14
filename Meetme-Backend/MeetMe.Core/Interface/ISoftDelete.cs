using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Interface
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
