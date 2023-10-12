using MeetMe.Core.Dtos;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.InMemoryData
{

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

    public class InMemoryDataProvider : IPersistenceProvider
    {
        private readonly object _lockObjectRef = new object();
        private readonly InMemoryDatabase _inMemoryDatabase;

        public InMemoryDataProvider(InMemoryDatabase inMemoryDatabase)
        {
            _inMemoryDatabase = inMemoryDatabase;
        }

        #region Appointment
        public async Task<Appointment?> GetAppointment(Guid id)
        {
            lock (_lockObjectRef)
            {
                var appointment = _inMemoryDatabase.AppointmentData.FirstOrDefault(e => e.Id == id);

                return appointment;
            }
        }
        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            lock (_lockObjectRef)
            {
                var appointment = GetAppointment(id).Result;
                var eventType = GetEventTypeById(appointment.EventTypeId).Result;
                var eventOwner = GetUserById(eventType.OwnerId).Result;

                var appointmentDetails = AppointmentDetailsDto.New(appointment, eventType, eventOwner!);

                return appointmentDetails;
            }
        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            lock (_lockObjectRef)
            {
                var listEventType = _inMemoryDatabase.EventTypeData.Where(e => e.OwnerId == userId).ToList();
                var listAppointment = _inMemoryDatabase.AppointmentData.Where(e => listEventType.Any(x => x.Id == e.EventTypeId)).ToList();
                var user = GetUserById(userId).Result;

                List<AppointmentDetailsDto> listAppointmentDetailsDto = new List<AppointmentDetailsDto>();

                foreach (var appointment in listAppointment)
                {
                    var appointmentDetailDto = AppointmentDetailsDto.New(appointment, listEventType.First(e => e.Id == appointment.EventTypeId), user!);

                    listAppointmentDetailsDto.Add(appointmentDetailDto);
                }

                return listAppointmentDetailsDto;
            }
        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            lock (_lockObjectRef)
            {
                var eventType = _inMemoryDatabase.EventTypeData.FirstOrDefault(e => e.Id == eventTypeId);

                var listAppointment = _inMemoryDatabase.AppointmentData
                    .Where(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC)
                    .ToList();

                var user = GetUserById(eventType!.OwnerId).Result;


                List<AppointmentDetailsDto> listAppointmentDetailsDto = new List<AppointmentDetailsDto>();

                foreach (var appointment in listAppointment)
                {
                    var appointmentDetailDto = AppointmentDetailsDto.New(appointment, eventType, user!);

                    listAppointmentDetailsDto.Add(appointmentDetailDto);
                }

                return listAppointmentDetailsDto;
            }
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            lock (_lockObjectRef)
            {
                var index = _inMemoryDatabase.AppointmentData.FindIndex(e => e.Id == id);

                if (index != -1)
                {
                    _inMemoryDatabase.AppointmentData.RemoveAt(index);
                }

                return true;
            }
        }
        public async Task<bool> AddAppointment(Appointment appointment)
        {
            lock (_lockObjectRef)
            {
                _inMemoryDatabase.AppointmentData.Add(appointment);

                return true;
            }
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            lock (_lockObjectRef)
            {
                var findIndex = _inMemoryDatabase.AppointmentData.FindIndex(e => e.Id == appointment.Id);

                if (findIndex != -1)
                {
                    _inMemoryDatabase.AppointmentData.RemoveAt(findIndex);
                }
                _inMemoryDatabase.AppointmentData.Add(appointment);

                return true;
            }
        }
        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            lock (_lockObjectRef)
            {
                return _inMemoryDatabase.AppointmentData
                   .Any(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC);
            }
        }

        #endregion

        #region Availability
        public async Task<Availability?> GetAvailability(Guid id)
        {
            lock (_lockObjectRef)
            {
                var availability = _inMemoryDatabase.AvailabilityData.FirstOrDefault(e => e.Id == id);
                return availability;
            }
        }
        public async Task<bool> AddAvailability(Availability availability)
        {
            lock (_lockObjectRef)
            {
                _inMemoryDatabase.AvailabilityData.Add(availability);

                return true;
            }
        }
        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            lock (_lockObjectRef)
            {
                var availability = _inMemoryDatabase.AvailabilityData.FirstOrDefault(e => e.Id == id);

                if (availability != null)
                {
                    _inMemoryDatabase.AvailabilityData.ForEach(e => e.IsDefault = false);

                    availability.IsDefault = true;
                }

                return true;
            }
        }

        public async Task<bool> UpdateAvailability(Availability availability)
        {
            lock (_lockObjectRef)
            {
                var findIndex = _inMemoryDatabase.AvailabilityData.FindIndex(e => e.Id == availability.Id);

                if (findIndex != -1)
                {
                    _inMemoryDatabase.AvailabilityData.RemoveAt(findIndex);
                }
                _inMemoryDatabase.AvailabilityData.Add(availability);

                return true;
            }
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            lock (_lockObjectRef)
            {
                var availability = GetAvailability(id).Result;
                availability.Name = nameToUpdate;

                return true;
            }
        }

        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo)
        {
            lock (_lockObjectRef)
            {
                var result = UpdateEventType(eventTypeInfo).Result;

                return true;
            }
        }
        public async Task<bool> DeleteAvailability(Availability availability)
        {
            lock (_lockObjectRef)
            {
                var index = _inMemoryDatabase.AvailabilityData.FindIndex(e => e.Id == availability.Id);

                if (index != -1)
                {
                    _inMemoryDatabase.AppointmentData.RemoveAt(index);
                }

                return true;
            }
        }

        #endregion

        #region event type

        public async Task<EventType?> GetEventTypeById(Guid eventTypeId)
        {
            lock (_lockObjectRef)
            {
                var eventType = _inMemoryDatabase.EventTypeData.FirstOrDefault(e => e.Id == eventTypeId);
                return eventType;
            }
        }
        public async Task<List<EventTypeQuestion>?> GetQuestionsByEventId(Guid eventTypeId)
        {
            lock (_lockObjectRef)
            {
                var listQuestions = _inMemoryDatabase.EventQuestionData.Where(e => e.EventTypeId == eventTypeId).ToList();

                return listQuestions;
            }
        }
        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            lock (_lockObjectRef)
            {
                _inMemoryDatabase.EventTypeData.Add(eventTypeInfo);

                return true;
            }
        }

        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            lock (_lockObjectRef)
            {
                var findIndex = _inMemoryDatabase.EventTypeData.FindIndex(e => e.Id == eventTypeInfo.Id);

                if (findIndex != -1)
                {
                    _inMemoryDatabase.EventTypeData.RemoveAt(findIndex);
                }

                _inMemoryDatabase.EventTypeData.Add(eventTypeInfo);

                return true;
            }

        }

        public async Task<EventType?> GetEventTypeBySlug(string slug)
        {
            lock (_lockObjectRef)
            {
                var eventType = _inMemoryDatabase.EventTypeData.FirstOrDefault(e => e.Slug == slug);
                return eventType;
            }
        }

        public async Task<List<EventType>?> GetEventTypeListByUserId(Guid userId)
        {
            lock (_lockObjectRef)
            {
                var listEventTypes = _inMemoryDatabase.EventTypeData.Where(e => e.OwnerId == userId).ToList();
                return listEventTypes;
            }
        }

        public async Task<bool> ResetEventQuestions(Guid eventTypeId, List<EventTypeQuestion> questions)
        {
            lock (_lockObjectRef)
            {
                if (_inMemoryDatabase.EventQuestionData.Any(e => e.EventTypeId == eventTypeId))
                {
                    _inMemoryDatabase.EventQuestionData.RemoveAll(e => e.EventTypeId == eventTypeId);
                }

                _inMemoryDatabase.EventQuestionData.AddRange(questions);

                return true;
            }
        }

        #endregion


        #region User

        public async Task<User?> GetUserBySlug(string slug)
        {
            lock (_lockObjectRef)
            {
                var user = _inMemoryDatabase.UserData.FirstOrDefault(e => e.BaseURI == slug);
                return user;
            }
        }

        public async Task<User?> GetUserById(Guid id)
        {
            lock (_lockObjectRef)
            {
                var user = _inMemoryDatabase.UserData.FirstOrDefault(e => e.Id == id);
                return user;
            }
        }

        public async Task<User?> GetUserByLoginId(string userId)
        {
            lock (_lockObjectRef)
            {
                var user = _inMemoryDatabase.UserData.FirstOrDefault(e => e.UserId == userId);
                return user;
            }
        }

        public async Task<List<Availability>?> GetListByUserId(Guid userId)
        {
            lock (_lockObjectRef)
            {
                return _inMemoryDatabase.AvailabilityData.Where(e => e.OwnerId == userId).ToList();
            }
        }
        public async Task<List<User>?> GetUserList()
        {
            lock (_lockObjectRef)
            {
                return _inMemoryDatabase.UserData;
            }
        }

        public async Task<bool> IsUserSlugAvailable(string slug, Guid id)
        {
            lock (_lockObjectRef)
            {
                var entity =  _inMemoryDatabase.UserData
                                .FirstOrDefault(e => e.BaseURI.ToLower() == slug.ToLower());

                if (entity == null) return true;

                return entity.Id == id;
            }
        }
        public async Task<bool> UpdateUser(User userEntity)
        {
            lock (_lockObjectRef)
            {
                var findIndex = _inMemoryDatabase.UserData.FindIndex(e => e.Id == userEntity.Id);

                if (findIndex != -1)
                {
                    _inMemoryDatabase.UserData.RemoveAt(findIndex);
                }
                _inMemoryDatabase.UserData.Add(userEntity);

                return true;
            }
        }

        public async Task<bool> AddNewUser(User userEntity)
        {
            lock (_lockObjectRef)
            {
                _inMemoryDatabase.UserData.Add(userEntity);

                return true;
            }

        }
        public void EnsureDbCreated()
        {
        }

        #endregion

    }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

}
