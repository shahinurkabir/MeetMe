using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class EventTypeEntityConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventType");
            builder.HasKey(e => e.Id);
            builder.Property(b => b.Name).IsRequired();
            builder.HasOne(e => e.User)
               .WithMany(e => e.EventTypes)
               .HasForeignKey(f => f.OwnerId)
               .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
