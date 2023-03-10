using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class EventTypeAvailabilityEntityConfiguration : IEntityTypeConfiguration<EventTypeAvailability>
    {
        public void Configure(EntityTypeBuilder<EventTypeAvailability> builder)
        {
            builder.ToTable("EventTypeAvailability");
            builder.HasKey(e => e.Id);
            builder.HasOne(e=>e.EventType)
                .WithOne(e => e.EventTypeAvailability)
                .HasForeignKey<EventTypeAvailability>(f=>f.Id)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class EventTypeAvailabilityDetailsEntityConfiguration : IEntityTypeConfiguration<EventTypeAvailabilityDetail>
    {
        public void Configure(EntityTypeBuilder<EventTypeAvailabilityDetail> builder)
        {
            builder.ToTable("EventTypeAvailabilityDetail");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Availability)
                .WithMany(e => e.AvailabilityDetails)
                .HasForeignKey(f => f.AvailabilityId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
