using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventQuestionRepository
    {
        Task<List<EventTypeQuestion>> GetQuestionsByEventId(Guid eventTypeId);
        Task <bool> ResetEventQuestions(Guid eventTypeId,List<EventTypeQuestion> questions);
    }

}