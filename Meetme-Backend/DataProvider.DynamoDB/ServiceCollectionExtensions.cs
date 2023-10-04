using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Interface;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DataProvider.DynamoDB
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseAWSDynamoDB(this IServiceCollection services, string accessKey, string secretKey, string endpoint_url, string region_name)
        {
            services.AddScoped<AmazonDynamoDBClient>(sp => new AmazonDynamoDBClient(accessKey, secretKey, new AmazonDynamoDBConfig { ServiceURL = endpoint_url, AuthenticationRegion = region_name }));
            services.AddScoped<DynamoDbInitializer, DynamoDbInitializer>();
            services.AddScoped<IDynamoDBContext>(sp => new DynamoDBContext(sp.GetRequiredService<AmazonDynamoDBClient>()));
            services.AddScoped<IPersistenceProvider, DynamoDbPersistenceProvider>();
            return services;

        }
    }
}
