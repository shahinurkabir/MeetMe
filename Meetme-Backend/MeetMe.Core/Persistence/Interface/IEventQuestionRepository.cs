using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventQuestionRepository
    {
        Task<List<EventTypeQuestion>> GetQuestionsByEventId(Guid eventTypeId);
        Task ResetEventQuestions(List<EventTypeQuestion> questions);
    }

}