using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Interface;

namespace DataProvider.InMemoryData
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseInMemoryData(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDatabase, InMemoryDatabase>();
            services.AddSingleton<IPersistenceProvider, InMemoryDataProvider>();

            return services;

        }
    }
}
