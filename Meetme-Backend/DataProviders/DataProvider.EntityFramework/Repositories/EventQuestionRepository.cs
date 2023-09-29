using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Repositories
{
    public class EventQuestionRepository : IEventQuestionRepository
    {
        private readonly MeetMeDbContext bookingDbContext;

        public EventQuestionRepository(MeetMeDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }
        public async Task<List<EventTypeQuestion>> GetQuestionsByEventId(Guid eventTypeId)
        {
            return await bookingDbContext.Set<EventTypeQuestion>()
                .Where(e => e.EventTypeId == eventTypeId)
                .ToListAsync();
        }

        public async Task<bool> ResetEventQuestions(Guid eventTypeId,List<EventTypeQuestion> questions)
        {
            var listOfQuestion = await bookingDbContext.Set<EventTypeQuestion>()
               .Where(e => e.EventTypeId == eventTypeId && e.SystemDefinedYN==false)
               .ToListAsync();

            if (listOfQuestion.Any())
            {
                bookingDbContext.RemoveRange(listOfQuestion);

            }
            await bookingDbContext.AddRangeAsync(questions);

            await bookingDbContext.SaveChangesAsync();

            return true;
        }

    }
}
