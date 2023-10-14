using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MeetMe.Core.Persistence.Interface;
using DataProvider.EntityFramework.Repositories;
using MeetMe.Core.Services;

namespace DataProvider.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseEFCoreSQLServer(this IServiceCollection  services,string connectionString)
        {
            services.AddDbContext<MeetMeDbContext>(option => option.UseSqlServer(connectionString));
            services.AddScoped<IPersistenceProvider, PersistenceProviderEntityFrameWork>();
            services.AddScoped<SeedDataService, SeedDataService>();

            return services;

        }
        public static IServiceCollection UseEFCoreSQLite(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MeetMeDbContext>(option => option.UseSqlite(connectionString));
            services.AddScoped<IPersistenceProvider, PersistenceProviderEntityFrameWork>();
            services.AddScoped<SeedDataService, SeedDataService>();

            return services;

        }
    }
}
