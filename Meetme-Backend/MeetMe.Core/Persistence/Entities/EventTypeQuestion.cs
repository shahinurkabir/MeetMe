using System;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventTypeQuestion
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string? Options { get; set; }
        public bool OtherOptionYN { get; set; }
        public bool ActiveYN { get; set; }
        public bool RequiredYN { get; set; }
        public short DisplayOrder { get; set; }
        public bool SystemDefinedYN { get; set; }
        public EventType? EventType { get; set; } 
    }
}

