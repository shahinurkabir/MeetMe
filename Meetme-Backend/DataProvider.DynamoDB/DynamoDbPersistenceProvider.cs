using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace DataProvider.DynamoDB
{
    public class DynamoDbPersistenceProvider : IPersistenceProvider
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly AmazonDynamoDBClient amazonDynamoDBClient;
        private readonly DynamoDbInitializer dynamoDbInitializer;

        public DynamoDbPersistenceProvider(IDynamoDBContext dynamoDBContext, AmazonDynamoDBClient amazonDynamoDBClient, DynamoDbInitializer dynamoDbInitializer)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.amazonDynamoDBClient = amazonDynamoDBClient;
            this.dynamoDbInitializer = dynamoDbInitializer;
        }

        public void EnsureDbCreated()
        {
            var _ = dynamoDbInitializer.EnsureDbCreated().Result;
        }

        #region Users
        public async Task<User?> GetUserById(Guid id)
        {
            var user = await dynamoDBContext.LoadAsync<User>(id);
            return user;
        }

        public async Task<User?> GetUserByLoginId(string userId)
        {
            var response = await dynamoDBContext
                .QueryAsync<User>(userId, new DynamoDBOperationConfig { IndexName = "UserIDIndex" })
                .GetRemainingAsync();

            return response.FirstOrDefault();
        }

        public async Task<User?> GetUserBySlug(string slug)
        {
            var response = await dynamoDBContext
                 .QueryAsync<User>(slug, new DynamoDBOperationConfig { IndexName = "SlugIndex" })
                 .GetRemainingAsync();

            return response.FirstOrDefault();
        }

        public async Task<List<User>?> GetUserList()
        {
            var scanConfig = new ScanOperationConfig
            {
                Limit = 1
            };
            var response = await dynamoDBContext.FromScanAsync<User>(scanConfig).GetRemainingAsync();
            return response;
        }
        public async Task<bool> AddNewUser(User userEntity)
        {
            await dynamoDBContext.SaveAsync(userEntity);
            return true;
        }
        public async Task<bool> UpdateUser(User userEntity)
        {
            await dynamoDBContext.SaveAsync(userEntity);
            return true;
        }
        public async Task<bool> IsUserSlugAvailable(string slug, Guid id)
        {
            var entity = await GetUserBySlug(slug);

            if (entity == null) return true;

            return entity.Id == id;
        }

        #endregion

        #region Availability
        public async Task<bool> AddAvailability(Availability availability)
        {
            await dynamoDBContext.SaveAsync(availability);
            return true;
        }
        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveAsync(eventTypeInfo);
            return true;
        }
        public async Task<bool> DeleteAvailability(Availability availability)
        {
            availability.IsDeleted = true;

            // detach all event type availability those are related from this schedule rule
            var listEventType = await GetEventTypeListByUserId(availability.OwnerId);

            if (listEventType != null)
            {
                foreach (var eventTypeItem in listEventType)
                {
                    if (eventTypeItem.AvailabilityId == availability.Id)
                    {
                        eventTypeItem.AvailabilityId = null;
                    }
                }
                await dynamoDBContext.SaveAsync(listEventType);
            }

            await dynamoDBContext.SaveAsync(availability);



            return true;
        }
        public async Task<Availability?> GetAvailability(Guid id)
        {
            var availability = await dynamoDBContext.LoadAsync<Availability>(id);
            return availability;
        }
        public async Task<List<Availability>?> GetListByUserId(Guid userId)
        {
            var response = await dynamoDBContext
                .QueryAsync<Availability>(userId, new DynamoDBOperationConfig { IndexName = "OwnerIdIndex" })
                .GetRemainingAsync();

            return response;
        }
        public async Task<bool> UpdateAvailability(Availability availability)
        {
            await dynamoDBContext.SaveAsync(availability);
            return true;
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            var entity = await dynamoDBContext.LoadAsync<Availability>(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = nameToUpdate;

            await dynamoDBContext.SaveAsync(entity);

            return true;
        }

        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetListByUserId(userId);

            if (listAvailabilityForUser == null)
            {
                return false;
            }
            var entity = listAvailabilityForUser?.FirstOrDefault(e => e.Id == id);

            if (entity == null)

            {
                return false;
            }

            listAvailabilityForUser?.ForEach(e => e.IsDefault = false);

            entity.IsDefault = true;

            var batchWritter = dynamoDBContext.CreateBatchWrite<Availability>();
            batchWritter.AddPutItems(listAvailabilityForUser);
            await batchWritter.ExecuteAsync();

            return true;
        }

        #endregion

        #region Event Type

        public async Task<EventType?> GetEventTypeById(Guid eventTypeId)
        {

            var eventType = await dynamoDBContext.LoadAsync<EventType>(eventTypeId);
            return eventType;
        }

        public async Task<EventType?> GetEventTypeBySlug(string slug)
        {
            var response = await dynamoDBContext
                .QueryAsync<EventType>(slug, new DynamoDBOperationConfig { IndexName = "SlugIndex" })
                .GetRemainingAsync();

            return response.FirstOrDefault();
        }

        public async Task<List<EventType>?> GetEventTypeListByUserId(Guid userId)
        {
            var response = await dynamoDBContext
                .QueryAsync<EventType>(userId, new DynamoDBOperationConfig { IndexName = "OwnerIdIndex" })
                .GetRemainingAsync();

            return response;
        }
        public Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateEventAvailability(EventType eventTypeInfo)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ResetEventQuestions(Guid eventTypeId, List<EventTypeQuestion> questions)
        {
            throw new NotImplementedException();
        }

        public Task<List<EventTypeQuestion>?> GetQuestionsByEventId(Guid eventTypeId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Appointments

        public async Task<Appointment?> GetAppointment(Guid id)
        {
            var appointment = await dynamoDBContext.LoadAsync<Appointment>(id);
            return appointment;
        }
        public Task<bool> AddAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteAppointment(Guid id)
        {
            await dynamoDBContext.DeleteAsync<Appointment>(id);
            return true;
        }

        public Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            throw new NotImplementedException();
        }
        #endregion

    }

}