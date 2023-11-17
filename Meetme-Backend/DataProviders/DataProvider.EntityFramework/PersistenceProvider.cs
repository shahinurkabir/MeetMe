using MeetMe.Core.Constants;
using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Repositories
{
    public class PersistenceProviderEntityFrameWork : IPersistenceProvider
    {
        private readonly MeetMeDbContext _dbContext;

        public PersistenceProviderEntityFrameWork(MeetMeDbContext bookingDbContext)
        {
            _dbContext = bookingDbContext;
        }
        public async Task<bool> AddAvailability(Availability availability)
        {
            await _dbContext.AddAsync(availability);
            _dbContext.SaveChanges();

            return true;

        }

        public async Task<bool> UpdateAvailability(Availability availability)
        {
            var listScheduleLineItem = await _dbContext.Set<AvailabilityDetail>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            if (listScheduleLineItem.Any())
            {
                _dbContext.RemoveRange(listScheduleLineItem);
            }

            _dbContext.Update(availability);

            // update time schedule of all event type availability those are related to this schedule rule
            var listEventType = await _dbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .Include(e => e.EventTypeAvailabilityDetails)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                _dbContext.RemoveRange(eventTypeItem.EventTypeAvailabilityDetails);

                eventTypeItem.EventTypeAvailabilityDetails.AddRange(availability.Details.Select(e => new EventTypeAvailabilityDetail
                {
                    EventTypeId = eventTypeItem.Id,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To,
                    StepId = e.StepId,
                }));
            }

            _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAvailability(Availability availability)
        {
            availability.IsDeleted = true;

            // detach all event type availability those are related from this schedule rule
            var listEventType = await _dbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.AvailabilityId = null;
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Availability>?> GetAvailabilityListByUser(Guid userId)
        {
            var list = await GetAvailabilityList(userId);

            return list;
        }

        public async Task<Availability?> GetAvailability(Guid ruleId)
        {
            var scheduleRule = await _dbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.Id == ruleId)
                .FirstOrDefaultAsync();

            return scheduleRule;
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            var entity = await _dbContext.Set<Availability>()
                .FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = nameToUpdate;

            await _dbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetAvailabilityList(userId);

            if (listAvailabilityForUser == null || !listAvailabilityForUser.Any())
            {
                return false;
            }

            var entity = listAvailabilityForUser.FirstOrDefault(e => e.Id == id);

            if (entity == null)

            {
                return false;
            }

            listAvailabilityForUser.ForEach(e => e.IsDefault = e.Id == id);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        private async Task<List<Availability>> GetAvailabilityList(Guid userId)
        {
            var list = await _dbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return list;
        }

        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await _dbContext.AddAsync(eventTypeInfo);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<EventType?> GetEventTypeById(Guid eventTypeId)
        {
            var entity = await _dbContext.Set<EventType>()
                .Where(e => e.Id == eventTypeId)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<EventType?> GetEventTypeBySlug(string slug)
        {
            var entity = await _dbContext.Set<EventType>()
                .Where(e => e.Slug == slug)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            _dbContext.Update(eventTypeInfo);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo)
        {
            var existingAvailabilityDetails = await _dbContext.Set<EventTypeAvailabilityDetail>()
              .Where(e => e.EventTypeId == eventTypeInfo.Id)
              .ToListAsync();

            if (existingAvailabilityDetails.Any())
            {
                _dbContext.RemoveRange(existingAvailabilityDetails);

            }

            _dbContext.Update(eventTypeInfo);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<EventType>?> GetEventTypeListByUser(Guid userId)
        {
            var result = await _dbContext.Set<EventType>()
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<List<EventTypeQuestion>?> GetQuestionsByEventId(Guid eventTypeId)
        {
            return await _dbContext.Set<EventTypeQuestion>()
                .Where(e => e.EventTypeId == eventTypeId)
                .ToListAsync();
        }

        public async Task<bool> ResetEventQuestions(Guid eventTypeId, List<EventTypeQuestion> questions)
        {
            var listOfQuestion = await _dbContext.Set<EventTypeQuestion>()
               .Where(e => e.EventTypeId == eventTypeId && e.SystemDefinedYN == false)
               .ToListAsync();

            if (listOfQuestion.Any())
            {
                _dbContext.RemoveRange(listOfQuestion);

            }
            await _dbContext.AddRangeAsync(questions);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await _dbContext.Set<Appointment>().AddAsync(appointment);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            await _dbContext.Set<Appointment>().Where(x => x.Id == id).ExecuteDeleteAsync();
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Appointment?> GetAppointment(Guid id)
        {
            return await _dbContext.Set<Appointment>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
        {
            return await _dbContext.Set<Appointment>()
                .AnyAsync(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC);
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            _dbContext.Set<Appointment>().Update(appointment);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            var result = await _dbContext.Set<Appointment>()
                 .Where(x => x.Id == id)
                 .Include(x => x.EventType)
                 .ThenInclude(x => x.User)
                 .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                 .FirstOrDefaultAsync();

            return result;

        }

        public async Task<List<AppointmentDetailsDto>> GetAppointmentListByUser(Guid userId)
        {
            var result = await _dbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventType!.User.Id == userId)
                  .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                  .ToListAsync();

            return result;
        }
        public async Task<List<AppointmentDetailsDto>> GetAppointmentListByEventType(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
        {
            var result = await _dbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.Status != Events.AppointmentStatus.Cancelled &&
                              x.EventTypeId == eventTypeId &&
                              x.StartTimeUTC >= startDateUTC &&
                              x.EndTimeUTC <= endDateUTC
                        )
                  .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                  .ToListAsync();

            return result;
        }
        public async Task<(int, List<AppointmentDetailsDto>?)> GetAppintmentListByParameters(AppointmentSearchParametersDto searchParametersDto, int pageNumber, int pageSize)
        {
            var entity = _dbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .AsQueryable();

            entity = entity.Where(x => x.OwnerId == searchParametersDto.OwnerId);

            if (!string.IsNullOrWhiteSpace(searchParametersDto.Status))
            {
                entity = entity.Where(x => x.Status == searchParametersDto.Status);
            }
            if (searchParametersDto.EventTypeIds.Any())
            {
                entity = entity.Where(x => searchParametersDto.EventTypeIds.Contains( x.EventTypeId));
            }
            entity = entity
                    .Where(e => e.StartTimeUTC >= searchParametersDto.StartDate && e.EndTimeUTC <= searchParametersDto.EndDate)
                    .OrderByDescending(e => e.StartTimeUTC);

            var totalRecords = await entity.CountAsync();

            var result = await entity.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                .ToListAsync();

            return (totalRecords, result);

        }

        public async Task<User?> GetUserByLoginId(string userId)
        {
            var user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return user;
        }

        public async Task<User?> GetUserBySlug(string slug)
        {
            var entity = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(e => e.BaseURI.Equals(slug));
            return entity;
        }

        public async Task<bool> IsUserSlugAvailable(string slug, Guid id)
        {
            var entity = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(e => e.BaseURI.Equals(slug));

            if (entity == null) return true;

            return entity.Id == id;
        }
        public async Task<List<User>?> GetUserList()
        {
            var list = await _dbContext.Set<User>().ToListAsync();
            return list;
        }

        public async Task<bool> UpdateUser(User userEntity)
        {
            _dbContext.Set<User>().Update(userEntity);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var userEntity = await _dbContext.Set<User>().FindAsync(id);
            return userEntity;
        }

        public async Task<bool> AddNewUser(User userEntity)
        {

            this._dbContext.Set<User>().Add(userEntity);
            await this._dbContext.SaveChangesAsync();

            return true;

        }

        public void EnsureDbCreated()
        {
            this._dbContext.Database.EnsureCreated();

            if (_dbContext.Set<User>().Any()) return;

        }



    }
}
