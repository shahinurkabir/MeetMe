using MeetMe.Core.Constants;
using static MeetMe.Core.Constants.Enums;

namespace MeetMe.Core.Dtos
{
    public class AppointmentSearchParametersDto
    {
        public AppointmentSearchParametersDto()
        {
            EventTypeIds = new List<Guid>();
            StatusNames = new List<string>();
        }
        public Guid OwnerId { get; set; }

        public string TimeZone { get; set; } = null!;

        /// <summary>
        /// P:Past
        /// U:Upcoming
        /// D:Date Range
        /// </summary>
        public string? Period { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// A:All
        /// E:Event Type
        /// S:Status
        /// I:Invitee Email
        /// </summary>
       // public string FilterBy { get; set; }// = "A";
        public List<Guid> EventTypeIds { get; set; }
        public List<string> StatusNames { get; set; }// = Events.AppointmentStatus.Active;
        public string? InviteeEmail { get; set; }
        public int PageNumber { get; set; }// = 1;
        
    }

    public class PaginationInfo
    {
        public PaginationInfo()
        {
            PagerLinks = new List<int>();
        }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public string CurrentPageDataRangeText { get; set; } = null!;
        public List<int> PagerLinks { get; set; }
    }

}
