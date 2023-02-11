using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IEventQuestionRepository
    {
        Task<List<EventTypeQuestion>> GetEventQuestionsByEventTypeId(Guid eventTypeId);
        Task ResetEventQuestions(List<EventTypeQuestion> questions);
        //Task AddNewEventQuestion(EventTypeQuestion eventTypeQuestion);
        //Task AddNewEventQuestions(List<EventTypeQuestion> questions);
        //Task UpdateEventQuestionOfEventType(EventTypeQuestion question);
        //Task DeleteEventQuestions(Guid eventTypeId);
    }

}