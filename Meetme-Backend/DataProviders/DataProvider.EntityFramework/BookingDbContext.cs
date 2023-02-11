using DataProvider.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework
{
    public class BookingDbContext : DbContext
    {
        public DbSet<EventType> EventType { get; set; }
        public DbSet<EventTypeQuestion> EventTypeQuestion { get; set; }
        public BookingDbContext(DbContextOptions<BookingDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof( EventTypeEntityConfiguration).Assembly);

        }
    }
}
