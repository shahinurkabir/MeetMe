using MeetMe.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments"); // Set the table name
        builder.HasKey(a => a.Id); // Set the primary key

        // Configure properties
        builder.Property(a => a.Id)
               .IsRequired()
               .HasColumnType("uniqueidentifier");

        builder.Property(a => a.EventTypeId)
               .IsRequired()
               .HasColumnType("uniqueidentifier");

        builder.Property(a => a.InviteeName)
               .IsRequired()
               .HasColumnType("varchar(100)");

        builder.Property(a => a.InviteeEmail)
               .IsRequired()
               .HasColumnType("varchar(100)");

        builder.Property(a => a.InviteeTimeZone)
               .IsRequired()
               .HasColumnType("varchar(100)");

        builder.Property(a => a.GuestEmails)
               .HasColumnType("varchar(255)");

        builder.Property(a => a.StartTimeUTC)
               .IsRequired()
               .HasColumnType("datetime2");

        builder.Property(a => a.EndTimeUTC)
               .IsRequired()
               .HasColumnType("datetime2");

        builder.Property(a => a.Status)
               .IsRequired()
               .HasColumnType("varchar(15)");

        builder.Property(a => a.DateCreated)
               .IsRequired()
               .HasColumnType("datetime2");

        builder.Property(a => a.Note)
               .HasColumnType("varchar(255)");

        builder.Property(a => a.DateCancelled)
               .HasColumnType("datetime2");

        builder.Property(a => a.CancellationReason)
               .HasColumnType("varchar(255)");

        // Configure the many-to-one relationship with EventType
        builder.HasOne(a => a.EventType)
               .WithMany(et => et.Appointments)
               .HasForeignKey(a => a.EventTypeId)
               .OnDelete(DeleteBehavior.Restrict);

    }
}
