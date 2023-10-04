using MeetMe.Core.Constants;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Services
{
    public class SeedDataService
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly IDateTimeService dateTimeService;

        public SeedDataService(IPersistenceProvider persistenceProvider, IDateTimeService dateTimeService)
        {
            this.persistenceProvider = persistenceProvider;
            this.dateTimeService = dateTimeService;
        }

        public async Task<bool> RunAsync(string userTimeZoneName)
        {
            Guid adminUserId = await AddAdminUser(userTimeZoneName);

            Guid availabilityId = await AddNewDefaultAvilability(adminUserId, userTimeZoneName);

            await AddNewEventType(adminUserId, availabilityId);

            return true;
        }


        public async Task<bool> IsDataSeededAsync()
        {
            var result = await persistenceProvider.GetUserList();

            return result != null && result.Any();
        }

        private async Task<Guid> AddAdminUser(string timeZoneName)
        {
            var adminUserId = Guid.NewGuid();
            var _ = await persistenceProvider.AddNewUser(new Persistence.Entities.User
            {
                Id = adminUserId,
                UserID = "admin",
                Password = "123",
                UserName = "administrator",
                BaseURI = "admin",
                TimeZone = timeZoneName,
                WelcomeText = "Please do book an appointment to talk about something."

            });
            return adminUserId;
        }
        private async Task<Guid> AddNewDefaultAvilability(Guid adminUserId, string timeZoneName)
        {
            var availabilityId = Guid.NewGuid();

            var result = await persistenceProvider.AddAvailability(new Availability
            {
                Id = availabilityId,
                IsDefault = true,
                IsDeleted = false,
                Name = "Availability Schedule - Default",
                OwnerId = adminUserId,
                TimeZone = timeZoneName,
                Details = Events.WeekDays.Select(day => new AvailabilityDetail
                {
                    DayType = Events.SCHEDULE_DATETYPE_WEEKDAY,
                    Value = day.Value,
                    From = 540,// 9AM
                    To = 1020, // 5PM
                    StepId = 0,
                }).ToList()
            });
            return availabilityId;
        }

        private async Task AddNewEventType(Guid adminUserId, Guid availabilityId)
        {
            var newEventTypeId = Guid.NewGuid();

            var defaultAvailability = await persistenceProvider.GetAvailability(availabilityId);


            EventType eventTypeInfo = MapCommandToEntity(newEventTypeId, defaultAvailability!, adminUserId);

            await persistenceProvider.AddNewEventType(eventTypeInfo);

            await Task.CompletedTask;
        }
        private EventType MapCommandToEntity(Guid eventTypeId, Availability availability, Guid adminUserId)
        {
            var listScheduleDetails = MapDefaultScheduleToEntity(eventTypeId, availability.Details);

            var listQuestions = GetDefaultQuestion();

            return new EventType
            {
                Id = eventTypeId,
                Name = "30 Minutes Meeting",
                OwnerId = adminUserId,
                Description = "30 Minutes Meeting",
                EventColor = "Green",
                Slug = "30-min",
                ActiveYN = true,
                TimeZone = availability.TimeZone,
                AvailabilityId = availability.Id,
                DateForwardKind = Events.ForwandDateKInd.Moving,
                ForwardDuration = Events.ForwardDuration,
                Duration = Events.MeetingDuration,
                BufferTimeBefore = Events.BufferTimeDuration,
                BufferTimeAfter = Events.BufferTimeDuration,
                CreatedBy = adminUserId,
                CreatedAt = dateTimeService.GetCurrentTimeUtc,
                EventTypeAvailabilityDetails = listScheduleDetails,
                Questions = listQuestions
            };
        }

        private List<EventTypeAvailabilityDetail> MapDefaultScheduleToEntity(Guid eventTypeId, List<AvailabilityDetail> availabilityDetails)
        {
            return availabilityDetails.Select(e => new EventTypeAvailabilityDetail
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                DayType = e.DayType,
                Value = e.Value,
                From = e.From,
                To = e.To,
                StepId = e.StepId
            }).ToList();

        }

        private List<EventTypeQuestion> GetDefaultQuestion()
        {
            var questions = new List<EventTypeQuestion>()
            {
                new EventTypeQuestion
                {
                    Name="Name",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=1 ,
                    RequiredYN=true,
                    SystemDefinedYN=true,
                },
                 new EventTypeQuestion
                {
                    Name="Email",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=2 ,
                    RequiredYN=true,
                    SystemDefinedYN=true,
                }
            };

            return questions;
        }
        private Availability GetDefaultAvailability(Guid userId, string timeZoneName)
        {
            var availabilityId = Guid.NewGuid();

            var availability = new Availability
            {
                Id = availabilityId,
                IsDefault = true,
                IsDeleted = false,
                Name = "Availability Schedule - Default",
                OwnerId = userId,
                TimeZone = timeZoneName,
                Details = Events.WeekDays.Select(day => new AvailabilityDetail
                {
                    DayType = Events.SCHEDULE_DATETYPE_WEEKDAY,
                    Value = day.Value,
                    From = 540,// 9AM
                    To = 1020, // 5PM
                    StepId = 0,
                }).ToList()
            };

            return availability;

        }


    }
}


