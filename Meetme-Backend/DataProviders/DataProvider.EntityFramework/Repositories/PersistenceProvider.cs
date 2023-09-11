using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Repositories
{
    //public class PersistenceProviderEntityFrameWork : IPersistenceProvider
    //{
    //    public Task AddAppointment(Appointment appointment)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task AddNewEventType(EventType eventTypeInfo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> AddSchedule(Availability scheduleRule)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> DeleteAppointment(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> DeleteSchedule(Availability scheduleRule)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> EditName(Guid id, string nameToUpdate)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<Appointment>> GetAppointmentsByDateRange(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<User?> GetByBaseURI(string URI)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<Availability> GetById(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<User?> GetByUserId(string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<EventTypeAvailabilityDetail>> GetEventTypeAvailabilityByEventId(Guid eventTypeId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<EventType> GetEventTypeById(Guid eventTypeId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<EventType>> GetEventTypeListByUserId(Guid userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<User>> GetList()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<Availability>> GetListByUserId(Guid userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<List<EventTypeQuestion>> GetQuestionsByEventId(Guid eventTypeId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task InsertItems(List<EventTypeAvailabilityDetail> itemsToInsert)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> IsLinkAvailable(string link, Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> IsTimeConflicting(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task RemoveItems(List<EventTypeAvailabilityDetail> itemsToRemove)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task ResetEventQuestions(Guid eventTypeId, List<EventTypeQuestion> questions)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> SetDefault(Guid id, Guid userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Update(User userEntity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> UpdateAppointment(Appointment appointment)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task UpdateEventAvailability(EventType eventTypeInfo, List<EventTypeAvailabilityDetail> eventTypeAvailabilityDetails)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task UpdateEventType(EventType eventTypeInfo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> UpdateSchedule(Availability scheduleRule)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task<Appointment> IAppointmentsRepository.GetById(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task<User?> IUserRepository.GetById(Guid id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
