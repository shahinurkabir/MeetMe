using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    using MeetMe.Core.Persistence.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class EventTypeQuestionConfiguration : IEntityTypeConfiguration<EventTypeQuestion>
    {
        public void Configure(EntityTypeBuilder<EventTypeQuestion> builder)
        {
            builder.ToTable("EventTypeQuestions"); // Set the table name
            builder.HasKey(eq => eq.Id); // Set the primary key

            // Configure properties
            builder.Property(eq => eq.Id).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(eq => eq.EventTypeId).IsRequired().HasColumnType("uniqueidentifier");
            builder.Property(eq => eq.Name).IsRequired().HasColumnType("varchar(255)");
            builder.Property(eq => eq.QuestionType).IsRequired().HasColumnType("varchar(30)");
            builder.Property(eq => eq.Options).HasColumnType("varchar(255)");
            builder.Property(eq => eq.OtherOptionYN).HasColumnType("bit");
            builder.Property(eq => eq.ActiveYN).HasColumnType("bit");
            builder.Property(eq => eq.RequiredYN).HasColumnType("bit");
            builder.Property(eq => eq.DisplayOrder).HasColumnType("smallint");
            builder.Property(eq => eq.SystemDefinedYN).HasColumnType("bit");

            // Configure the many-to-one relationship with EventType
            builder.HasOne(eq => eq.EventType)
                   .WithMany(et => et.Questions)
                   .HasForeignKey(eq => eq.EventTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }




}
