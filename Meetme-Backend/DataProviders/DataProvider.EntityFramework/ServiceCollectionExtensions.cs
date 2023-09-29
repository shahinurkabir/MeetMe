using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MeetMe.Core.Persistence.Interface;
using DataProvider.EntityFramework.Repositories;

namespace DataProvider.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseEFCoreSQLServer(this IServiceCollection  services,string connectionString)
        {
            services.AddDbContext<MeetMeDbContext>(option => option.UseSqlServer(connectionString));

            //services.AddScoped<IEventTypeRepository, EventTypeRepository>();
            //services.AddScoped<IEventQuestionRepository, EventQuestionRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
            //services.AddScoped<IAppointmentRepository,AppointmentRepository>();
            services.AddScoped<IPersistenceProvider, PersistenceProviderEntityFrameWork>();

            return services;

        }
    }
}
