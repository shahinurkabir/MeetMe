using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users"); // Set the table name
            builder.HasKey(u => u.Id); // Set the primary key

            // Configure properties
            builder.Property(u => u.Id).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(u => u.UserID).IsRequired().HasColumnType("varchar(30)");
            builder.Property(u => u.Password).IsRequired().HasColumnType("varchar(30)");
            builder.Property(u => u.BaseURI).IsRequired().HasColumnType("varchar(30)");
            builder.Property(u => u.TimeZone).IsRequired().HasColumnType("varchar(100)");
            builder.Property(u => u.UserName).IsRequired().HasColumnType("varchar(100)");
            builder.Property(u => u.WelcomeText).HasColumnType("varchar(255)");

            // Configure the one-to-many relationship with EventType
            builder.HasMany(u => u.EventTypes)
                   .WithOne(et => et.User)
                   .HasForeignKey(et => et.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
