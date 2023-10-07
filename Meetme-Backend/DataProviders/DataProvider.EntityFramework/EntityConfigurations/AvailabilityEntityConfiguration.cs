using MeetMe.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.ToTable("Availabilities"); // Set the table name
        builder.HasKey(a => a.Id); // Set the primary key

        // Configure properties
        builder.Property(a => a.Id).IsRequired().HasColumnType("uniqueidentifier");
        builder.Property(a => a.Name).IsRequired().HasColumnType("varchar(100)");
        builder.Property(a => a.OwnerId).IsRequired().HasColumnType("uniqueidentifier");
        builder.Property(a => a.TimeZone).IsRequired().HasColumnType("varchar(100)");
        builder.Property(a => a.IsDefault).HasColumnType("bit");
        builder.Property(a => a.IsDeleted).HasColumnType("bit");

        // Configure one-to-many relationship with AvailabilityDetail
        builder.HasMany(a => a.Details)
               .WithOne(ad => ad.Availability)
               .HasForeignKey(ad => ad.AvailabilityId);
               //.OnDelete(DeleteBehavior.Restrict);

    }
}
