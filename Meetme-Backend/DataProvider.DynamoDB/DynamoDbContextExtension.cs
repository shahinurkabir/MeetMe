using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MeetMe.Core.Interface;
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
            var config = new DynamoDBOperationConfig {};

            AddSoftDeleteQueryFilter<T>(config);

            var item = await dynamoDBContext.LoadAsync<T>(id,config);

            return item;
        }
        public static async Task<T?> GetItem<T>(this IDynamoDBContext dynamoDBContext, object haskKeyValue, string indexName)
        {
            var config = new DynamoDBOperationConfig { IndexName = indexName };

            AddSoftDeleteQueryFilter<T>(config);

            var items = await dynamoDBContext
                .QueryAsync<T>(haskKeyValue, config)
                .GetRemainingAsync();

            return items.FirstOrDefault();

        }

        public static async Task<List<T>?> GetList<T>(this IDynamoDBContext dynamoDBContext, object haskKeyValue, string indexName, params ScanCondition[] scanConditions)
        {
            var config = new DynamoDBOperationConfig { IndexName = indexName, QueryFilter = scanConditions.ToList() };
            
            AddSoftDeleteQueryFilter<T>(config);

            var items = await dynamoDBContext
                .QueryAsync<T>(haskKeyValue,config)
                .GetRemainingAsync();

            return items;

        }

        public static void AddSoftDeleteQueryFilter<T>(DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            var typeInfo = typeof(T);

            if (typeof(ISoftDelete).IsAssignableFrom(typeInfo))
            {
                var propertyName = nameof(ISoftDelete.IsDeleted);
                var softDeleteCondition = new ScanCondition(propertyName, ScanOperator.Equal, false);

                if (dynamoDBOperationConfig.QueryFilter == null)
                {
                    dynamoDBOperationConfig.QueryFilter = new List<ScanCondition> { softDeleteCondition };
                }
                else
                {
                    dynamoDBOperationConfig.QueryFilter.Add(softDeleteCondition);
                }
            }
        }
    }
}
