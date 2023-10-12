using Amazon.DynamoDBv2.DataModel;
using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.DynamoDB
{
    public static class DynamoDbContextExtension
    {
        public static async Task<bool> SaveChangesAsync<T>(this IDynamoDBContext dynamoDBContext, T entityToSave)
        {
            await dynamoDBContext.SaveAsync(entityToSave);
            return true;
        }
        public static async Task<bool> SaveChangesAsync<T>(this IDynamoDBContext dynamoDBContext, List<T> entitiesToSave)
        {
            var batchWritter = dynamoDBContext.CreateBatchWrite<T>();

            batchWritter.AddPutItems(entitiesToSave);

            await batchWritter.ExecuteAsync();
            
            return true;
        }

        public static async Task<T?> GetById<T>(this IDynamoDBContext dynamoDBContext, object id)
        {
            var item = await dynamoDBContext.LoadAsync<T>(id);
            return item;
        }
        public static async Task<T?> GetItem<T>(this IDynamoDBContext dynamoDBContext, object haskKeyValue, string indexName)
        {
            var items = await dynamoDBContext
                .QueryAsync<T>(haskKeyValue, new DynamoDBOperationConfig { IndexName = indexName })
                .GetRemainingAsync();

            return items.FirstOrDefault();

        }

        public static async Task<List<T>?> GetList<T>(this IDynamoDBContext dynamoDBContext, object haskKeyValue, string indexName, params ScanCondition[] scanConditions)
        {
            var items = await dynamoDBContext
                .QueryAsync<T>(haskKeyValue, new DynamoDBOperationConfig { IndexName = indexName, QueryFilter = scanConditions.ToList() })
                .GetRemainingAsync();

            return items;

        }
    }
}
