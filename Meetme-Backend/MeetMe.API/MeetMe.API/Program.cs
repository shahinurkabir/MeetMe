using DataProvider.EntityFramework;
using MeetMe.API.Middlewares;
using MeetMe.API.Models;
using MeetMe.Application;
using MeetMe.Application.Services;
using MeetMe.Caching.InMemory;
using MeetMe.Core.Interface;
using MeetMe.Core.Interface.Caching;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MeetMe API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var connectionString = builder.Configuration.GetConnectionString("BookingDB");

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();

//var domainAssembly = typeof(CreateEventTypeCommand).Assembly;
//builder.Services.AddMediatR(domainAssembly);

//Add FluentValidationbuilder.Sservices.AddFluentValidation(new[] { domainAssembly });

builder.Services.AddScoped<IUserInfo, ApplicationUser>();
//services.AddSingleton<IPersistenseProvider, InMemoryPersistenseProvider>();

//builder.Services.AddScoped<IEventTypeRepository, EventTypeRepository>();
//builder.Services.AddScoped<IEventQuestionRepository, EventQuestionRepository>();
//builder.Services.AddScoped<IEventAvailabilityRepository, EventAvailabilityRepository>();
//builder.Services.AddScoped<ITimeZoneDataRepository, TimeZoneDataRepository>();
//builder.Services.AddScoped<IPersistenceProvider, PersistenceProviderEF>();
builder.Services.AddScoped<ICacheService, InMemoryCacheService>();
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
//builder.Services.AddScoped<IEventTypeService, EventTypeService>();

builder.Services.RegisterApplicationDependency();
builder.Services.RegisterInfraDependency(connectionString);


builder.Services.AddCors(e => e.AddPolicy("AllowAll",
             builder =>
             {
                 builder
                 .AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
             }));



var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
