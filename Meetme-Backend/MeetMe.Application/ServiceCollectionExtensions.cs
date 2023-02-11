using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using MeetMe.Application.EventTypes.Create;
using Microsoft.Extensions.DependencyInjection;
namespace MeetMe.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationDependency(this IServiceCollection services)
        {
            var assembly = typeof(CreateEventTypeCommand).Assembly;

            services.AddMediatR(assembly);

            services.AddFluentValidation(new[] { typeof(CreateEventTypeCommand).Assembly });

            return services;
        }
    }
}
