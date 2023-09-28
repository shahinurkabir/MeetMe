using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
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
    public class PersistenceProviderEntityFrameWork : IPersistenceProvider
    {
        private readonly BookingDbContext _bookingDbContext;

        public PersistenceProviderEntityFrameWork(BookingDbContext bookingDbContext)
        {
            _bookingDbContext = bookingDbContext;
        }
        public async Task<bool> AddAvailability(Availability availability)
        {
            await _bookingDbContext.AddAsync(availability);
            _bookingDbContext.SaveChanges();

            return true;

        }

        public async Task<bool> UpdateAvailability(Availability availability)
        {
            var listScheduleLineItem = await _bookingDbContext.Set<AvailabilityDetail>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            _bookingDbContext.RemoveRange(listScheduleLineItem);

            _bookingDbContext.Update(availability);

            // update time schedule of all event type availability those are related to this schedule rule
            var listEventType = await _bookingDbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .Include(e => e.EventTypeAvailabilityDetails)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.EventTypeAvailabilityDetails.Clear();
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

            _bookingDbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAvailability(Availability availability)
        {
            availability.IsDeleted = true;

            // detach all event type availability those are related from this schedule rule
            var listEventType = await _bookingDbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.AvailabilityId = null;
            }

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Availability>> GetListByUserId(Guid userId)
        {
            var list = await GetAvailabilityList(userId);

            return list;
        }

        public async Task<Availability> GetAvailability(Guid ruleId)
        {
            var scheduleRule = await _bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.Id == ruleId)
                .FirstOrDefaultAsync();

            return scheduleRule;
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            var entity = await _bookingDbContext.Set<Availability>()
                .FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = nameToUpdate;

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetAvailabilityList(userId);

            var entity = listAvailabilityForUser.FirstOrDefault(e => e.Id == id);

            if (entity == null)

            {
                return false;
            }

            //Reset all availability to non-default
            listAvailabilityForUser.ForEach(e => e.IsDefault = false);

            entity.IsDefault = true;

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }

        private async Task<List<Availability>> GetAvailabilityList(Guid userId)
        {
            var list = await _bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return list;
        }

        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await _bookingDbContext.AddAsync(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }

        public async Task<EventType> GetEventTypeById(Guid eventTypeId)
        {
            var entity = await _bookingDbContext.Set<EventType>()
                .Where(e => e.Id == eventTypeId)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<EventType> GetEventTypeBySlug(string slug)
        {
            var entity = await _bookingDbContext.Set<EventType>()
                .Where(e => e.Slug == slug)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            _bookingDbContext.Update(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo, List<EventTypeAvailabilityDetail> scheduleDetails)
        {
            var eventTypeId = eventTypeInfo.Id;

            var existingAvailabilityDetails = await _bookingDbContext.Set<EventTypeAvailabilityDetail>()
              .Where(e => e.EventTypeId == eventTypeId)
              .ToListAsync();

            if (existingAvailabilityDetails.Any())
            {
                _bookingDbContext.RemoveRange(existingAvailabilityDetails);

            }
            await _bookingDbContext.AddRangeAsync(scheduleDetails);

            _bookingDbContext.Update(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<EventType>> GetEventTypeListByUserId(Guid userId)
        {
            var result = await _bookingDbContext.Set<EventType>()
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<List<EventTypeQuestion>> GetQuestionsByEventId(Guid eventTypeId)
        {
            return await _bookingDbContext.Set<EventTypeQuestion>()
                .Where(e => e.EventTypeId == eventTypeId)
                .ToListAsync();
        }

        public async Task<bool> ResetEventQuestions(Guid eventTypeId, List<EventTypeQuestion> questions)
        {
            var listOfQuestion = await _bookingDbContext.Set<EventTypeQuestion>()
               .Where(e => e.EventTypeId == eventTypeId && e.SystemDefinedYN == false)
               .ToListAsync();

            if (listOfQuestion.Any())
            {
                _bookingDbContext.RemoveRange(listOfQuestion);

            }
            await _bookingDbContext.AddRangeAsync(questions);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await _bookingDbContext.Set<Appointment>().AddAsync(appointment);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            await _bookingDbContext.Set<Appointment>().Where(x => x.Id == id).ExecuteDeleteAsync();
            return await _bookingDbContext.SaveChangesAsync() > 0;
        }

        public async Task<Appointment> GetAppointment(Guid id)
        {
            return await _bookingDbContext.Set<Appointment>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            return await _bookingDbContext.Set<Appointment>()
                .AnyAsync(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC);
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            _bookingDbContext.Set<Appointment>().Update(appointment);
            return await _bookingDbContext.SaveChangesAsync() > 0;
        }

        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                 .Where(x => x.Id == id)
                 .Include(x => x.EventType)
                 .ThenInclude(x => x.User)
                 .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                 .FirstOrDefaultAsync();

            return result;

        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventType.User.Id == userId)
                  .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                  .ToListAsync();

            return result;
        }
        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC)
                  .Select(x => AppointmentDetailsDto.New(x,x.EventType!,x.EventType!.User))
                  .ToListAsync();

            return result;
        }

        public async Task<User?> GetUserByLoginId(string userId)
        {
            var user = await _bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(x => x.UserID == userId);

            return user;
        }

        public async Task<User?> GetUserBySlug(string URI)
        {
            var entity = await _bookingDbContext.Set<User>()
                .FirstOrDefaultAsync(e => e.BaseURI.ToLower() == URI.ToLower());
            return entity;
        }

        public async Task<bool> IsUserSlugAvailable(string URI, Guid id)
        {
            var entity = await _bookingDbContext.Set<User>()
                            .FirstOrDefaultAsync(e => e.BaseURI.ToLower() == URI.ToLower());

            if (entity == null) return true;

            return entity.Id == id;
        }
        public async Task<List<User>> GetUserList()
        {
            var list = await _bookingDbContext.Set<User>().ToListAsync();
            return list;
        }

        public async Task<bool> UpdateUser(User userEntity)
        {
            _bookingDbContext.Set<User>().Update(userEntity);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var userEntity = await _bookingDbContext.Set<User>().FindAsync(id);
            return userEntity;
        }
        //private static AppointmentDetailsDto ToAppointmentDto(Appointment x)
        //{

        //    var dto = new AppointmentDetailsDto
        //    {
        //        Id = x.Id,
        //        EventTypeId = x.EventTypeId,
        //        InviteeName = x.InviteeName,
        //        InviteeEmail = x.InviteeEmail,
        //        StartTimeUTC = x.StartTimeUTC,
        //        EndTimeUTC = x.EndTimeUTC,
        //        InviteeTimeZone = x.InviteeTimeZone,
        //        GuestEmails = x.GuestEmails,
        //        Note = x.Note,
        //        Status = x.Status.ToString(),
        //        DateCreated = x.DateCreated,
        //        DateCancelled = x.DateCancelled,
        //        CancellationReason = x.CancellationReason,
        //        EventTypeTitle = x.EventType.Name,
        //        EventTypeDescription = x.EventType.Description,
        //        EventTypeLocation = x.EventType.Location,
        //        EventTypeDuration = x.EventType.Duration,
        //        EventTypeColor = x.EventType.EventColor,
        //        EventTypeTimeZone = x.EventType.TimeZone,
        //        EventOwnerId = x.EventType.OwnerId,
        //        EventOwnerName = x.EventType.User.UserName,
        //        AppointmentDateTime = x.InviteeTimeZone.ToAppointmentTimeRangeText(x.EventType.Duration, x.StartTimeUTC),
        //    };

        //    return dto;
        //}

    }
}
