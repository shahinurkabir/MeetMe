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
        private readonly string tableName_User = DynamoDbTableIndexConstants.User.TableName;
        private readonly string tableName_Availability = DynamoDbTableIndexConstants.Availability.TableName;
        private readonly string tableName_EventType = DynamoDbTableIndexConstants.EventType.TableName;
        private readonly string tableName_Appointment = DynamoDbTableIndexConstants.Appointment.TableName;


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
                    AttributeName = DynamoDbTableIndexConstants.Appointment.PrimaryKey,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.EventTypeId,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.Status,
                    AttributeType = ScalarAttributeType.S
                }
                ,
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.AppointmentDate,
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.PrimaryKey,
                    KeyType = KeyType.HASH // Primary key
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName,DynamoDbTableIndexConstants.Appointment.EventTypeId),
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.EventTypeId,
                    KeyType = KeyType.HASH
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
                }
                },
                new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName,DynamoDbTableIndexConstants.Appointment.Status),
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.Status,
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
                },
                 new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Appointment.TableName,DynamoDbTableIndexConstants.Appointment.AppointmentDate),
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = DynamoDbTableIndexConstants.Appointment.AppointmentDate,
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
                }
            },


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
                    AttributeName = DynamoDbTableIndexConstants.EventType.PrimaryKey,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.EventType.Slug,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.EventType.AvailabilityId,
                    AttributeType = ScalarAttributeType.S
                },
                 new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.EventType.OwnerId,
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = DynamoDbTableIndexConstants.Availability.PrimaryKey,
                    KeyType = KeyType.HASH // Primary key
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.EventType.TableName,DynamoDbTableIndexConstants.EventType.Slug),
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = DynamoDbTableIndexConstants.EventType.Slug,
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
                }
                },
                new GlobalSecondaryIndex
                {
                 IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.EventType.TableName,DynamoDbTableIndexConstants.EventType.AvailabilityId),
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName =DynamoDbTableIndexConstants.EventType.AvailabilityId,
                    KeyType = KeyType.HASH // GSI hash key
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
                },
                 new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.EventType.TableName,DynamoDbTableIndexConstants.EventType.OwnerId),
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName =DynamoDbTableIndexConstants.EventType.OwnerId,
                    KeyType = KeyType.HASH
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
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
                    AttributeName = DynamoDbTableIndexConstants.Availability.PrimaryKey,
                    AttributeType = ScalarAttributeType.S
                },
                new AttributeDefinition
                {
                    AttributeName = DynamoDbTableIndexConstants.Availability.OwnerId,
                    AttributeType = ScalarAttributeType.S
                }
                },
                KeySchema = new List<KeySchemaElement>
                {
                new KeySchemaElement
                {
                    AttributeName = DynamoDbTableIndexConstants.Availability.PrimaryKey,
                    KeyType = KeyType.HASH
                }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                new GlobalSecondaryIndex
                {
                IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.Availability.TableName,DynamoDbTableIndexConstants.Availability.OwnerId),
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                    AttributeName = DynamoDbTableIndexConstants.Availability.OwnerId,
                    KeyType = KeyType.HASH
                }
                },
                Projection = new Projection
                {
                ProjectionType = ProjectionType.ALL
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
                }
                },

            }

            };

            await CreateTableAsync(request);
        }
        private async Task CreateUserTableAsync(string tableName)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition> {
                    new AttributeDefinition(DynamoDbTableIndexConstants.User.PrimaryKey, ScalarAttributeType.S),
                    new AttributeDefinition(DynamoDbTableIndexConstants.User.UserId, ScalarAttributeType.S),
                    new AttributeDefinition(DynamoDbTableIndexConstants.User.BaseURI, ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement(DynamoDbTableIndexConstants.User.PrimaryKey, KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>  {
                    new GlobalSecondaryIndex
                    {
                        IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.User.TableName, DynamoDbTableIndexConstants.User.UserId),
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement(DynamoDbTableIndexConstants.User.UserId, KeyType.HASH) // Hash key for GSI
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5,
                            WriteCapacityUnits = 5
                        }
                    },
                    new GlobalSecondaryIndex
                    {
                        IndexName = DynamoDbTableIndexConstants.GetIndexName(DynamoDbTableIndexConstants.User.TableName, DynamoDbTableIndexConstants.User.BaseURI),
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement(DynamoDbTableIndexConstants.User.BaseURI, KeyType.HASH) // Hash key for GSI
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        },
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5,
                            WriteCapacityUnits = 5
                        }
                    }
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
