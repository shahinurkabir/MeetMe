using DataProvider.DynamoDB;
using DataProvider.EntityFramework;
using DataProvider.InMemoryData;
using MeetMe.API.Middlewares;
using MeetMe.API.Models;
using MeetMe.Application;
using MeetMe.Caching.InMemory;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

RegisterJwtAuthentication(builder);

RegisterAuthorization(builder);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();

RegisterServices(builder);

//builder.Services.UseInMemoryData();
//builder.Services.UseEFCoreSQLServer(builder.Configuration.GetConnectionString("MeetMeDb")!);
//builder.Services.UseEFCoreSQLite(builder.Configuration.GetConnectionString("MeetMeDb-sqlite")!);
builder.Services.UseDynamoDB(builder.Configuration["AWSDynamoDB:AccessKey"], builder.Configuration["AWSDynamoDB:SecretKey"], builder.Configuration["AWSDynamoDB:EndpointUrl"], builder.Configuration["AWSDynamoDB:RegionName"]);

builder.Services.RegisterApplication();

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

    EnsureDbCreatedAddSeedingData(app, "Asia/Dhaka"); // To be able to change timezone later after login via profile page.
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void EnsureDbCreatedAddSeedingData(WebApplication builder, string timeZoneName)
{
    using var serviceScope = builder.Services.CreateScope();
    var dbProvider = serviceScope.ServiceProvider.GetRequiredService<IPersistenceProvider>();
    var seedDataService = serviceScope.ServiceProvider.GetRequiredService<SeedDataService>();

    dbProvider.EnsureDbCreated();

    var isDataSeeded = seedDataService.IsDataSeededAsync().Result;
    if (isDataSeeded)
    {
        return;
    }
    var result = seedDataService.RunAsync(timeZoneName).Result;
}

static void RegisterJwtAuthentication(WebApplicationBuilder builder)
{
    var tokenConfig = builder.Configuration.GetSection("JwtConfig");

    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfig["Secret"]));
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = true,
        ValidIssuer = tokenConfig["Issuer"],
        ValidateAudience = true,
        ValidAudience = tokenConfig["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RequireExpirationTime = true,
    };

    builder.Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = "bearer";
        o.DefaultChallengeScheme = "bearer";
    })
    .AddJwtBearer("bearer", x =>
    {
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = tokenValidationParameters;
    });
}

static void RegisterAuthorization(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(option =>
    {
        option.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
}

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    builder.Services.AddScoped<ILoginUserInfo, LoginUserInfo>();

    builder.Services.AddScoped<ICacheService, InMemoryCacheService>();
    builder.Services.AddScoped<IDateTimeService, DateTimeService>();
    builder.Services.AddScoped<SeedDataService, SeedDataService>();
}