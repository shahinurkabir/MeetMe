using MeetMe.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public class AvailabilityDetailConfiguration : IEntityTypeConfiguration<AvailabilityDetail>
{
    public void Configure(EntityTypeBuilder<AvailabilityDetail> builder)
    {
        builder.ToTable("AvailabilityDetails"); // Set the table name
        builder.HasKey(ad => ad.Id); // Set the primary key

        // Configure properties
        builder.Property(ad => ad.Id)
               .IsRequired()
               .HasColumnType("INTEGER")
               .ValueGeneratedOnAdd();

        builder.Property(ad => ad.AvailabilityId)
               .IsRequired()
               .HasColumnType("uniqueidentifier");

        builder.Property(ad => ad.DayType)
               .IsRequired()
               .HasColumnType("varchar(15)");

        builder.Property(ad => ad.Value)
               .IsRequired()
               .HasColumnType("varchar(15)");

        builder.Property(ad => ad.StepId)
               .HasColumnType("smallint");

        builder.Property(ad => ad.From)
               .HasColumnType("float");

        builder.Property(ad => ad.To)
               .HasColumnType("float");

        // Configure the many-to-one relationship with Availability
        builder.HasOne(ad => ad.Availability)
               .WithMany(a => a.Details)
               .HasForeignKey(ad => ad.AvailabilityId);
               //.OnDelete(DeleteBehavior.Restrict);

    }
}
