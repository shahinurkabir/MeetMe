using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
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
                .QueryAsync<User>(userId, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.User.TableName, DynamoDbTableIndexConstants.User.UserId) })
                .GetRemainingAsync();

            return response.FirstOrDefault();
        }

        public async Task<User?> GetUserBySlug(string slug)
        {
            var response = await dynamoDBContext
                 .QueryAsync<User>(slug, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.User.TableName,DynamoDbTableIndexConstants.User.BaseURI)  })
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
                .QueryAsync<Availability>(userId, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Availability.TableName, DynamoDbTableIndexConstants.Availability.OwnerId) })
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
                .QueryAsync<EventType>(slug, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.EventType.TableName, DynamoDbTableIndexConstants.EventType.Slug) })
                .GetRemainingAsync();

            return response.FirstOrDefault();
        }

        public async Task<List<EventType>?> GetEventTypeListByUserId(Guid userId)
        {
            var response = await dynamoDBContext
                .QueryAsync<EventType>(userId, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.EventType.TableName, DynamoDbTableIndexConstants.EventType.OwnerId) })
                .GetRemainingAsync();

            return response;
        }
        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveAsync(eventTypeInfo);
            return true;
        }
        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveAsync(eventTypeInfo);
            return true;
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
        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await dynamoDBContext.SaveAsync(appointment);
            return true;
        }

        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            // Build the query filter
            var queryFilter = new QueryFilter();
            queryFilter.AddCondition( DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName, DynamoDbTableIndexConstants.Appointment.EventTypeId), QueryOperator.Equal, eventTypeId);
            queryFilter.AddCondition(DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName, DynamoDbTableIndexConstants.Appointment.AppointmentDate), QueryOperator.Between, startDateUTC.DateTime, endDateUTC.DateTime);

            // Perform the query with the combined filter
            var listAppointment = await dynamoDBContext.QueryAsync<Appointment>(queryFilter).GetRemainingAsync();

            return listAppointment.Any();

        }
        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            await dynamoDBContext.SaveAsync(appointment);
            return true;
        }
        public async Task<bool> DeleteAppointment(Guid id)
        {
            await dynamoDBContext.DeleteAsync<Appointment>(id);
            return true;
        }

        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            var appointmentEntity = await dynamoDBContext.LoadAsync<Appointment>(id);

            if (appointmentEntity == null)
            {
                return null;
            }

            var eventTypeEntity = await dynamoDBContext.LoadAsync<EventType>(appointmentEntity.EventTypeId);
            var eventOwnerEntity = await dynamoDBContext.LoadAsync<User>(eventTypeEntity.OwnerId);

            var result = AppointmentDetailsDto.New(appointmentEntity, eventTypeEntity, eventOwnerEntity);

            return result;

        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            //TODO:this method is not optimized, need to be refactored

            var userRetrevalTask = GetUserById(userId);
            var eventTypeRetrevalTask = GetEventTypeListByUserId(userId);

            await Task.WhenAll(userRetrevalTask, eventTypeRetrevalTask);

            if (userRetrevalTask.Result == null || eventTypeRetrevalTask.Result == null)
            {
                return null;
            }

            var userEntity = userRetrevalTask.Result;
            var listEventType = eventTypeRetrevalTask.Result;

            var listAppointment = await dynamoDBContext
                .QueryAsync<Appointment>(userId, new DynamoDBOperationConfig { IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName,DynamoDbTableIndexConstants.Appointment.OwnerId) })
                .GetRemainingAsync();

            if (listAppointment == null)
            {
                return null;
            }

            var listAppointmentDetails = new List<AppointmentDetailsDto>();

            foreach (var appointment in listAppointment)
            {
                var eventTypeEntity = listEventType.FirstOrDefault(e => e.Id == appointment.EventTypeId);

                if (eventTypeEntity == null)
                {
                    continue;
                }

                var appointmentDetails = AppointmentDetailsDto.New(appointment, eventTypeEntity, userEntity);

                listAppointmentDetails.Add(appointmentDetails);
            }

            return listAppointmentDetails;
        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            //TODO:this method is not optimized, need to be refactored

            var eventType = await GetEventTypeById(eventTypeId);

            if (eventType == null)
            {
                return null;
            }

            var userRetrevalTask = GetUserById(eventType.OwnerId);
            var eventTypeRetrevalTask = GetEventTypeListByUserId(eventType.OwnerId);

            // Build the query filter
            var queryFilter = new QueryFilter();
            queryFilter.AddCondition(DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName, DynamoDbTableIndexConstants.Appointment.EventTypeId), QueryOperator.Equal, eventTypeId);
            queryFilter.AddCondition(DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName, DynamoDbTableIndexConstants.Appointment.AppointmentDate), QueryOperator.Between, startDateUTC.DateTime, endDateUTC.DateTime);

            // Perform the query with the combined filter
            var appointmentRetrevalTask = dynamoDBContext.QueryAsync<Appointment>(queryFilter).GetRemainingAsync();

            await Task.WhenAll(userRetrevalTask, appointmentRetrevalTask,eventTypeRetrevalTask);


            var userEntity = userRetrevalTask.Result;
            var listAppointment = appointmentRetrevalTask.Result;
            var listEventType = eventTypeRetrevalTask.Result;

            if (userEntity == null || listAppointment == null || listEventType == null)
            {
                return null;
            }
            var listAppointmentDetails = new List<AppointmentDetailsDto>();

            foreach (var appointment in listAppointment)
            {
                var eventTypeEntity = listEventType.FirstOrDefault(e => e.Id == appointment.EventTypeId);

                if (eventTypeEntity == null)
                {
                    continue;
                }

                var appointmentDetails = AppointmentDetailsDto.New(appointment, eventTypeEntity, userEntity);

                listAppointmentDetails.Add(appointmentDetails);
            }

            return listAppointmentDetails;
        }

        #endregion

    }

}