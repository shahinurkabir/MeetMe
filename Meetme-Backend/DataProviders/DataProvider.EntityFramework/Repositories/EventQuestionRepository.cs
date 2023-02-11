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
        private readonly BookingDbContext bookingDbContext;

        public EventQuestionRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }
        public async Task<List<EventTypeQuestion>> GetEventQuestionsByEventTypeId(Guid eventTypeId)
        {
            return await bookingDbContext.Set<EventTypeQuestion>()
                .Where(e => e.EventTypeId == eventTypeId)
                .ToListAsync();
        }

        public async Task ResetEventQuestions(List<EventTypeQuestion> questions)
        {
            var eventTypeId = questions.First().EventTypeId;

            var listOfQuestion = await bookingDbContext.Set<EventTypeQuestion>()
               .Where(e => e.EventTypeId == eventTypeId)
               .ToListAsync();

            if (listOfQuestion.Any())
            {
                bookingDbContext.RemoveRange(listOfQuestion);

            }
            await bookingDbContext.AddRangeAsync(questions);

            await bookingDbContext.SaveChangesAsync();
        }

    }
}
