using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.DynamoDB
{
    public  class DynamoDbTableAndIndexConstants
    {
        public const string User_Table = nameof(User);
        public const string User_PrimaryKey = nameof(User.Id);
        public const string User_UserId = nameof(User.UserId);
        public const string User_BaseURI= nameof(User.BaseURI);

        public const string EventType_Table =nameof(EventType);
        public const string EventType_PrimaryKey = nameof(EventType.Id);
        public const string EventType_OwnerId = nameof(EventType.OwnerId);
        public const string EventType_Slug = nameof(EventType.Slug);
        public const string EventType_AvailabilityId = nameof(EventType.AvailabilityId);

        public const string Availability_Table = nameof(Availability);
        public const string Availability_PrimaryKey = nameof(Availability.Id);
        public const string Availability_OwnerId = nameof(Availability.OwnerId);

        public const string Appointment_Table = nameof(Appointment);
        public const string Appointment_PrimaryKey = nameof(Appointment.Id);
        public const string Appointment_EventTypeId = nameof(Appointment.EventTypeId);
        public const string Appointment_AppointmentDate = nameof(Appointment.StartTimeUTC);
        public const string Appointment_Status = nameof(Appointment.Status);
        public const string Appointment_OwnerId = nameof(Appointment.OwnerId);
        public static string GetIndexName(string tableName, string indexName)
        {
            return $"{tableName}_{indexName}";
        }
    }
  
}
