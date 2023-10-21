using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventTypes"); // Set the table name
            builder.HasKey(e => e.Id); // Set the primary key

            // Configure properties with VARCHAR data types
            builder.Property(e => e.Id).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.OwnerId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.Name).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.Description).HasColumnType("varchar(255)");
            builder.Property(e => e.Location).HasColumnType("varchar(50)");
            builder.Property(e => e.Slug).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.EventColor).IsRequired().HasColumnType("varchar(30)");
            builder.Property(e => e.ActiveYN).HasColumnType("bit");
            builder.Property(e => e.IsDeleted).HasColumnType("bit");
            builder.Property(e => e.TimeZone).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.DateForwardKind).IsRequired().HasColumnType("varchar(30)");
            builder.Property(e => e.ForwardDuration).HasColumnType("int");
            builder.Property(e => e.DateFrom).HasColumnType("datetime2");
            builder.Property(e => e.DateTo).HasColumnType("datetime2");
            builder.Property(e => e.Duration).HasColumnType("int");
            builder.Property(e => e.BufferTimeBefore).HasColumnType("int");
            builder.Property(e => e.BufferTimeAfter).HasColumnType("int");
            builder.Property(e => e.AvailabilityId).HasColumnType("uniqueidentifier");
            builder.Property(e => e.CreatedBy).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(e => e.CreatedAt).IsRequired().HasColumnType("datetime2");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime2");
            builder.Property(e => e.UpdatedBy).HasColumnType("uniqueidentifier");

            // Configure relationships
            builder.HasMany(e => e.Questions)
                   .WithOne()
                   .HasForeignKey(q => q.EventTypeId);
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.EventTypeAvailabilityDetails)
                   .WithOne()
                   .HasForeignKey(ead => ead.EventTypeId);
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Appointments)
                   .WithOne(a => a.EventType)
                   .HasForeignKey(a => a.EventTypeId);
            //.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
            

        }
    }

}
