using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.DynamoDB
{
    public  class DynamoDbTableIndexConstants
    {

        public static string GetIndexName(string tableName, string indexName)
        {
            return $"{tableName}_{indexName}";
        }
        public static class User
        {
            public const string TableName = "Users";
            public const string PrimaryKey = "Id";
            public const string UserId = "UserId";
            public const string BaseURI = "BaseURI";
        }
        public static class EventType
        {
            public const string TableName = "EventTypes";
            public const string PrimaryKey = "Id";
            public const string OwnerId = "OwnerId";
            public const string Slug = "Slug";
            public const string AvailabilityId = "AvailabilityId";
        }
        public static class Availability
        {
            public const string TableName = "Availabilities";
            public const string PrimaryKey = "Id";
            public const string OwnerId = "OwnerId";
        }
        public static class Appointment
        {
            public const string TableName = "Appointments";
            public const string PrimaryKey = "Id";
            public const string EventTypeId = "EventTypeId";
            public const string AppointmentDate = "StartTimeUTC";
            public const string Status = "Status";
            public const string OwnerId = "OwnerId";
        }

    }
}
