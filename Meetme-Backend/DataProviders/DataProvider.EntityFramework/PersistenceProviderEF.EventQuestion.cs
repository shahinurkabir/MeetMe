using MeetMe.Core.Persistence.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework
{
    public partial class PersistenceProviderEF
    {
        //public async Task<List<EventTypeQuestion>> GetEventQuestionsByEventTypeId(Guid eventTypeId)
        //{
        //    return await bookingDbContext.Set<EventTypeQuestion>()
        //        .Where(e => e.EventTypeId == eventTypeId)
        //        .ToListAsync();
        //}
        //public async Task AddNewEventQuestion(EventTypeQuestion eventTypeQuestion)
        //{
        //    await bookingDbContext.AddAsync(eventTypeQuestion);

        //    await Task.FromResult(false);

        //}
        //public async Task AddNewEventQuestions(List<EventTypeQuestion> questions)
        //{
        //    await bookingDbContext.AddRangeAsync(questions);

        //    await Task.FromResult(false);

        //}
        //public async Task UpdateEventQuestionOfEventType(EventTypeQuestion question)
        //{
        //    bookingDbContext.Update(question);

        //    await Task.FromResult(false);
        //}
        //public async Task DeleteEventQuestion(EventTypeQuestion eventTypeQuestion)
        //{
        //    bookingDbContext.Remove(eventTypeQuestion);

        //    await Task.FromResult(false);
        //}

        //public async Task DeleteEventQuestions(Guid eventTypeId)
        //{
        //    var listOfQuestion = await bookingDbContext.Set<EventTypeQuestion>()
        //        .Where(e => e.EventTypeId == eventTypeId)
        //        .ToListAsync();

        //    if (listOfQuestion.Any())
        //    {
        //        bookingDbContext.RemoveRange(listOfQuestion);
        //    }

        //}
    }
}
