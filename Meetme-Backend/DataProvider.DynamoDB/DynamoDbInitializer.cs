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
        private readonly string tableName_User = nameof(User);
        private readonly string tableName_Availability = nameof(Availability);
        private readonly string tableName_EventType = nameof(EventType);
        private readonly string tableName_Appointment = nameof(Appointment);


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

        private async Task CreateAppointmentTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>  {
                new AttributeDefinition
                {
                    AttributeName = "Id",
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = "EventTypeId",
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = "Status",
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "Id",
                    KeyType = KeyType.HASH // Primary key
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,  // Adjust as needed
                    WriteCapacityUnits = 5  // Adjust as needed
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = "EventTypeIdIndex",
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = "EventTypeId",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,  // Adjust as needed
                WriteCapacityUnits = 5  // Adjust as needed
                }
                },
                new GlobalSecondaryIndex
                {
                IndexName = "StatusIndex",
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "Status",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,  // Adjust as needed
                    WriteCapacityUnits = 5  // Adjust as needed
                }
                }
            }

            };

            await CreateTableAsync(request);

        }
        private async Task CreateEventTypeTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>  {
                new AttributeDefinition
                {
                    AttributeName = "Id",
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = "Slug",
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = "AvailabilityId",
                    AttributeType = ScalarAttributeType.S
                },
                 new AttributeDefinition
                {
                    AttributeName = "OwnerId",
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "Id",
                    KeyType = KeyType.HASH // Primary key
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,  // Adjust as needed
                    WriteCapacityUnits = 5  // Adjust as needed
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = "SlugIndex",
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = "Slug",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,  // Adjust as needed
                WriteCapacityUnits = 5  // Adjust as needed
                }
                },
                new GlobalSecondaryIndex
                {
                IndexName = "AvailabilityIdIndex",
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "AvailabilityId",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,  // Adjust as needed
                    WriteCapacityUnits = 5  // Adjust as needed
                }
                },
                 new GlobalSecondaryIndex
                {
                IndexName = "OwnerIdIndex",
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = "OwnerId",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,  // Adjust as needed
                WriteCapacityUnits = 5  // Adjust as needed
                }
                },
            }

            };

            await CreateTableAsync(request);
        }
        private async Task CreateAvailabilityTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>  {
                new AttributeDefinition
                {
                    AttributeName = "Id",
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = "OwnerId",
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = "Id",
                    KeyType = KeyType.HASH // Primary key
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,  // Adjust as needed
                    WriteCapacityUnits = 5  // Adjust as needed
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = "OwnerIdIndex",
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = "OwnerId",
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL // Include all attributes
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,  // Adjust as needed
                WriteCapacityUnits = 5  // Adjust as needed
                }
                },

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
        private async Task CreateUserTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition> {
                    new AttributeDefinition("Id", ScalarAttributeType.S),
                    new AttributeDefinition("UserID", ScalarAttributeType.S),
                    new AttributeDefinition("BaseURI", ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement("Id", KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5, // Adjust read capacity units as needed
                    WriteCapacityUnits = 5 // Adjust write capacity units as needed
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>  {
                    new GlobalSecondaryIndex
                    {
                        IndexName = "UserIDIndex", // Replace with your desired index name
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement("UserID", KeyType.HASH) // Hash key for GSI
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL // Adjust projection type as needed
                        },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5, // Adjust read capacity for GSI
                            WriteCapacityUnits = 5 // Adjust write capacity for GSI
                        }
                    },
                    new GlobalSecondaryIndex
                    {
                        IndexName = "BaseURIIndex", // Replace with your desired index name
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement("BaseURI", KeyType.HASH) // Hash key for GSI
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL // Adjust projection type as needed
                        },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5, // Adjust read capacity for GSI
                            WriteCapacityUnits = 5 // Adjust write capacity for GSI
                        }
                    }
    }
            };

            // var response = await client.CreateTableAsync(request

            await CreateTableAsync(request);

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
