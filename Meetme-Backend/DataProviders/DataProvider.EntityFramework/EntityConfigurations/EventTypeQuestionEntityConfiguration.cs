using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class EventTypeQuestionEntityConfiguration : IEntityTypeConfiguration<EventTypeQuestion>
    {
        public void Configure(EntityTypeBuilder<EventTypeQuestion> builder)
        {
            builder.ToTable("EventTypeQuestion");
            builder.HasKey(e => e.Id);
            builder.Property(b => b.Name).IsRequired();
            builder.Property(b => b.QuestionType).IsRequired();
            builder.HasOne(e=>e.EventType)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
