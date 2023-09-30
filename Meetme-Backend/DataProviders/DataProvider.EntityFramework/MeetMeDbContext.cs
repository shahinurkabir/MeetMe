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
        //private readonly string connectionString;

        //public MeetMeDbContext(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}
        public MeetMeDbContext(DbContextOptions<MeetMeDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           // optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof( EventTypeConfiguration).Assembly);

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
