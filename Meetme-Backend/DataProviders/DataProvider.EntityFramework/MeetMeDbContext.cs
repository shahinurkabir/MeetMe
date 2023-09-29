using DataProvider.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;
using Microsoft.IdentityModel.Abstractions;
using MeetMe.Core.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using DataProvider.EntityFramework.Extensions;

namespace DataProvider.EntityFramework
{
    public class MeetMeDbContext : DbContext
    {
        public MeetMeDbContext(DbContextOptions<MeetMeDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof( EventTypeEntityConfiguration).Assembly);

            modelBuilder.Entity<EventType>().HasQueryFilter(e => !e.IsDeleted);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

        }
    }
}
