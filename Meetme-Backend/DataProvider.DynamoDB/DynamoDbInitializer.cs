using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon;
using System.Text.RegularExpressions;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.DynamoDB
{
    public class DynamoDbInitializer
    {
        private readonly AmazonDynamoDBClient client;
        private readonly string tableName_User = DynamoDbTableAndIndexConstants.User_Table;
        private readonly string tableName_Availability = DynamoDbTableAndIndexConstants.Availability_Table;
        private readonly string tableName_EventType = DynamoDbTableAndIndexConstants.EventType_Table;
        private readonly string tableName_Appointment = DynamoDbTableAndIndexConstants.Appointment_Table;


        public DynamoDbInitializer(AmazonDynamoDBClient client)
        {
            this.client = client;
        }
        public async Task<bool> EnsureDbCreated()
        {
            await CreateTables();

            return true;
        }
        public async Task CreateTables()
        {
            var listOfTaskToCreateTables = new List<Task>();

            if (!await IsExistAsync(tableName_User)) { listOfTaskToCreateTables.Add(CreateUserTableAsync(tableName_User)); }
            if (!await IsExistAsync(tableName_Availability)) { listOfTaskToCreateTables.Add(CreateAvailabilityTableAsync(tableName_Availability)); }
            if (!await IsExistAsync(tableName_EventType)) { listOfTaskToCreateTables.Add(CreateEventTypeTableAsync(tableName_EventType)); }
            if (!await IsExistAsync(tableName_Appointment)) { listOfTaskToCreateTables.Add(CreateAppointmentTableAsync(tableName_Appointment)); }


            await Task.WhenAll(listOfTaskToCreateTables);

        }

        private static AttributeDefinition GetAttributeDefinition(string attributeName, ScalarAttributeType type)
        {
            return new AttributeDefinition
            {
                AttributeName = attributeName,
                AttributeType = type
            };
        }
        private static KeySchemaElement GetKeySchemaElement(string attributeName, KeyType type)
        {
            return new KeySchemaElement
            {
                AttributeName = attributeName,
                KeyType = type
            };
        }
        private static GlobalSecondaryIndex GetGlobalSecondaryIndex(string tableName, string fieldName, KeyType keyType, ProjectionType projectionType, long readCapacity, long writeCapcity, params string[] rangeKeys)
        {
            var keySchema = new List<KeySchemaElement>();
            
            keySchema.Add(GetKeySchemaElement(fieldName, keyType));


            foreach (var r in rangeKeys)
            {
                keySchema.Add(GetKeySchemaElement(r, KeyType.RANGE));
            }
            return new GlobalSecondaryIndex
            {
                IndexName = DynamoDbTableAndIndexConstants.GetIndexName(tableName, fieldName),
                KeySchema = keySchema,
                Projection = new Projection
                {
                    ProjectionType = projectionType
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = readCapacity,
                    WriteCapacityUnits = writeCapcity
                }
            };


        }
        private async Task CreateAppointmentTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>  {
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Appointment_PrimaryKey,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Appointment_EventTypeId,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Appointment_Status,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Appointment_AppointmentDate,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Appointment_OwnerId,ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    GetKeySchemaElement(DynamoDbTableAndIndexConstants.Appointment_PrimaryKey,KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.Appointment_EventTypeId, KeyType.HASH, ProjectionType.ALL, 5, 5,DynamoDbTableAndIndexConstants.Appointment_AppointmentDate),
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.Appointment_Status, KeyType.HASH, ProjectionType.ALL, 5, 5),
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.Appointment_OwnerId, KeyType.HASH, ProjectionType.ALL, 5, 5),
                },

            };

            await CreateTableAsync(request);

        }
        private async Task CreateEventTypeTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.EventType_PrimaryKey,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.EventType_Slug,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.EventType_AvailabilityId,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.EventType_OwnerId,ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    GetKeySchemaElement(DynamoDbTableAndIndexConstants.EventType_PrimaryKey,KeyType.HASH)

                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.EventType_Slug, KeyType.HASH, ProjectionType.ALL, 5, 5),
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.EventType_AvailabilityId    , KeyType.HASH, ProjectionType.ALL, 5, 5),
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.EventType_OwnerId, KeyType.HASH, ProjectionType.ALL, 5, 5),
                }

            };

            await CreateTableAsync(request);
        }
        private async Task CreateAvailabilityTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Availability_PrimaryKey,ScalarAttributeType.S),
                    GetAttributeDefinition( DynamoDbTableAndIndexConstants.Availability_OwnerId,ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    GetKeySchemaElement(DynamoDbTableAndIndexConstants.Availability_PrimaryKey,KeyType.HASH)

                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.Availability_OwnerId, KeyType.HASH, ProjectionType.ALL, 5, 5),

                }

            };

            await CreateTableAsync(request);
        }
        private async Task CreateUserTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    GetAttributeDefinition(DynamoDbTableAndIndexConstants.User_PrimaryKey, ScalarAttributeType.S),
                    GetAttributeDefinition(DynamoDbTableAndIndexConstants.User_UserId, ScalarAttributeType.S),
                    GetAttributeDefinition(DynamoDbTableAndIndexConstants.User_BaseURI, ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    GetKeySchemaElement(DynamoDbTableAndIndexConstants.User_PrimaryKey, KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.User_UserId, KeyType.HASH, ProjectionType.ALL, 5, 5),
                    GetGlobalSecondaryIndex(tableName, DynamoDbTableAndIndexConstants.User_BaseURI, KeyType.HASH, ProjectionType.ALL, 5, 5)
                }
            };

            await CreateTableAsync(request);

        }
        private async Task<bool> IsExistAsync(string tableName)
        {
            try
            {
                await client.DescribeTableAsync(tableName);
                return true;
            }
            catch (ResourceNotFoundException)
            {
                return false;
            }
        }
        private async Task CreateTableAsync(CreateTableRequest createTableRequest)
        {
            var createTableResponse = await client.CreateTableAsync(createTableRequest);

            var isCompleted = false;
            var timeToLoop = 0;
            while (!isCompleted && timeToLoop < 20)
            {
                var describeResponse = await client.DescribeTableAsync(createTableRequest.TableName);
                if (describeResponse.Table.TableStatus == TableStatus.ACTIVE)
                {
                    isCompleted = true;
                }
                else
                {
                    await Task.Delay(3000);
                    timeToLoop++;
                }
            }


        }

    }
}
