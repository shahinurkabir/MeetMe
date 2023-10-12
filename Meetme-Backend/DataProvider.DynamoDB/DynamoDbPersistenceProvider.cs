using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System.Linq;
using Amazon;
using System.Runtime.InteropServices;
using DataProvider.DynamoDB;
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
            var indexName = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.User_Table, DynamoDbTableAndIndexConstants.User_UserId);

            var response = await dynamoDBContext.GetItem<User>(userId, indexName);

            return response;
        }

        public async Task<User?> GetUserBySlug(string slug)
        {
            var indexName = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.User_Table, DynamoDbTableAndIndexConstants.User_BaseURI);

            var response = await dynamoDBContext.GetItem<User>(slug, indexName);

            return response;
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
            await dynamoDBContext.SaveChangesAsync(userEntity);
            return true;
        }
        public async Task<bool> UpdateUser(User userEntity)
        {
            await dynamoDBContext.SaveChangesAsync(userEntity);
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
            await dynamoDBContext.SaveChangesAsync(availability);
            return true;
        }
        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveChangesAsync(eventTypeInfo);
            return true;
        }

        public async Task<bool> DeleteAvailability(Availability availability)
        {
            availability.IsDeleted = true;

            // Detach all event type availability that is related to this schedule rule
            var listEventType = await GetEventTypeListByUserId(availability.OwnerId);

            if (listEventType != null)
            {
                foreach (var eventTypeItem in listEventType.Where(item => item.AvailabilityId == availability.Id))
                {
                    eventTypeItem.AvailabilityId = null;
                }

                await dynamoDBContext.SaveChangesAsync(listEventType);
            }

            await dynamoDBContext.SaveChangesAsync(availability);

            return true;
        }

        public async Task<Availability?> GetAvailability(Guid id)
        {
            var availability = await dynamoDBContext.GetById<Availability>(id);
            return availability;
        }
        public async Task<List<Availability>?> GetListByUserId(Guid userId)
        {
            var indexName = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.Availability_Table, DynamoDbTableAndIndexConstants.Availability_OwnerId);

            var response = await dynamoDBContext.GetList<Availability>(userId, indexName);

            return response;
        }
        public async Task<bool> UpdateAvailability(Availability availability)
        {
            await dynamoDBContext.SaveChangesAsync(availability);
            return true;
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            var entity = await dynamoDBContext.GetById<Availability>(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = nameToUpdate;

            var result=await dynamoDBContext.SaveChangesAsync(entity);

            return result;
        }

        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetListByUserId(userId);

            if (listAvailabilityForUser == null || !listAvailabilityForUser.Any())
            {
                return false;
            }

            var entity = listAvailabilityForUser.FirstOrDefault(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            foreach (var availability in listAvailabilityForUser)
            {
                availability.IsDefault = availability.Id == id;
            }

            var result = await dynamoDBContext.SaveChangesAsync(listAvailabilityForUser);

            return true;
        }

        #endregion

        #region Event Type

        public async Task<EventType?> GetEventTypeById(Guid eventTypeId)
        {

            var eventType = await dynamoDBContext.GetById<EventType>(eventTypeId);
            return eventType;
        }

        public async Task<EventType?> GetEventTypeBySlug(string slug)
        {
            var indexName = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.EventType_Table, DynamoDbTableAndIndexConstants.EventType_Slug);

            var response = await dynamoDBContext.GetItem<EventType>(slug, indexName);

            return response;
        }

        public async Task<List<EventType>?> GetEventTypeListByUserId(Guid userId)
        {
            var indexName = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.EventType_Table, DynamoDbTableAndIndexConstants.EventType_OwnerId);

            var response = await dynamoDBContext.GetList<EventType>(userId, indexName);

            return response;
        }
        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveChangesAsync(eventTypeInfo);
            return true;
        }
        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo)
        {
            await dynamoDBContext.SaveChangesAsync(eventTypeInfo);
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
            var appointment = await dynamoDBContext.GetById<Appointment>(id);
            return appointment;
        }
        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await dynamoDBContext.SaveChangesAsync(appointment);
            return true;
        }

        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            var indexAppointmentEventTypeId = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.Appointment_Table, DynamoDbTableAndIndexConstants.Appointment_EventTypeId);

            var date1 = startDateUTC.UtcDateTime;
            var date2 = endDateUTC.UtcDateTime;

            var conditions = new List<ScanCondition>
            {
                new ScanCondition( DynamoDbTableAndIndexConstants.Appointment_EventTypeId, ScanOperator.Equal, eventTypeId),
                new ScanCondition(DynamoDbTableAndIndexConstants.Appointment_AppointmentDate, ScanOperator.Between, date1, date2)
            };

            var listAppointment = await dynamoDBContext.GetList<Appointment>(eventTypeId, indexAppointmentEventTypeId, conditions.ToArray());

            return listAppointment.Any();

        }
        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            await dynamoDBContext.SaveChangesAsync(appointment);
            return true;
        }
        public async Task<bool> DeleteAppointment(Guid id)
        {
            await dynamoDBContext.DeleteAsync<Appointment>(id);
            return true;
        }

        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            var appointmentEntity = await dynamoDBContext.GetById<Appointment>(id);

            if (appointmentEntity == null)
            {
                return null;
            }

            var eventTypeEntity = await dynamoDBContext.GetById<EventType>(appointmentEntity.EventTypeId);
            var eventOwnerEntity = await dynamoDBContext.GetById<User>(eventTypeEntity!.OwnerId);

            var result = AppointmentDetailsDto.New(appointmentEntity, eventTypeEntity, eventOwnerEntity!);

            return result;

        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            //TODO:this method is not optimized, need to be refactored

            var userEntityRequestTask = GetUserById(userId);
            var eventTypeRequestTask = GetEventTypeListByUserId(userId);

            await Task.WhenAll(userEntityRequestTask, eventTypeRequestTask);

            if (userEntityRequestTask.Result == null || eventTypeRequestTask.Result == null)
            {
                return null;
            }

            var userEntity = userEntityRequestTask.Result;
            var listEventType = eventTypeRequestTask.Result;
            var indexAppointmentOwnerId = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.Appointment_Table, DynamoDbTableAndIndexConstants.Appointment_OwnerId);

            var listAppointment = await dynamoDBContext.GetList<Appointment>(userId, indexAppointmentOwnerId);

            if (listAppointment == null)
            {
                return null;
            }

            List<AppointmentDetailsDto> listAppointmentDetails = ToConvertAppointDetailsViewModel(userEntity, listEventType, listAppointment);

            return listAppointmentDetails;
        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            //TODO:this method is not optimized, need to be refactored

            var indexAppointmentEventTypeId = DynamoDbTableAndIndexConstants.GetIndexName(DynamoDbTableAndIndexConstants.Appointment_Table, DynamoDbTableAndIndexConstants.Appointment_EventTypeId);
            var date1 = startDateUTC.UtcDateTime;
            var date2 = endDateUTC.UtcDateTime;
            var eventType = await GetEventTypeById(eventTypeId);

            if (eventType == null)
            {
                return null;
            }

            var userRetrevalTask = GetUserById(eventType.OwnerId);
            var eventTypeRetrevalTask = GetEventTypeListByUserId(eventType.OwnerId);


            var listConditions = new List<ScanCondition> {
                    new ScanCondition(DynamoDbTableAndIndexConstants.Appointment_EventTypeId, ScanOperator.Equal,eventTypeId),
                    new ScanCondition(DynamoDbTableAndIndexConstants.Appointment_AppointmentDate, ScanOperator.Between, startDateUTC.UtcDateTime, endDateUTC.UtcDateTime)
                };

            var appointmentRetrevalTask = dynamoDBContext.GetList<Appointment>(eventTypeId, indexAppointmentEventTypeId, listConditions.ToArray());

            await Task.WhenAll(userRetrevalTask, appointmentRetrevalTask, eventTypeRetrevalTask);


            var userEntity = userRetrevalTask.Result;
            var listAppointment = appointmentRetrevalTask.Result;
            var listEventType = eventTypeRetrevalTask.Result;

            if (userEntity == null || listAppointment == null || listEventType == null)
            {
                return null;
            }
            List<AppointmentDetailsDto> listAppointmentDetails = ToConvertAppointDetailsViewModel(userEntity, listEventType, listAppointment);

            return listAppointmentDetails;
        }
        private static List<AppointmentDetailsDto> ToConvertAppointDetailsViewModel(User userEntity, List<EventType> listEventType, List<Appointment> listAppointment)
        {
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