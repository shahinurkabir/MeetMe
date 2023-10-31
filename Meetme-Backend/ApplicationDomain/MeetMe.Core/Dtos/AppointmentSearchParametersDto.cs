using MeetMe.Core.Constants;
using static MeetMe.Core.Constants.Enums;

namespace MeetMe.Core.Dtos
{
    public class AppointmentSearchParametersDto
    {
        public AppointmentSearchParametersDto()
        {
            EventTypeIds = new List<Guid>();
        }
        public Guid OwnerId { get; set; }

        public string TimeZone { get; set; } = null!;

        /// <summary>
        /// P:Past
        /// U:Upcoming
        /// O:Ongoing
        /// D:Date Range
        /// </summary>
        public string SearchBy { get; set; } = "U";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// A:All
        /// E:Event Type
        /// S:Status
        /// I:Invitee Email
        /// </summary>
        public string FilterBy { get; set; } = "A";
        public List<Guid> EventTypeIds { get; set; }
        public string Status { get; set; } = Events.AppointmentStatus.Active;
        public string? InviteeEmail { get; set; }
        
    }

    public class PaginationInfo
    {
        public int PageSize { get; set; }= 20;
        public int PageNumber { get; set; }= 1;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
    }

}
