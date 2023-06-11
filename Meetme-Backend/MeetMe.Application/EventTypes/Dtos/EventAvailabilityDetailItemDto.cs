namespace MeetMe.Application.EventTypes.Update
{
    public class EventAvailabilityDetailItemDto
    {
        public string DayType { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int From { get; set; }
        public int To { get; set; }
        public short StepId { get; set; }
    }


}
