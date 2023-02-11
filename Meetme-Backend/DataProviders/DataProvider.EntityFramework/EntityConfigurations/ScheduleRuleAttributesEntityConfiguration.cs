using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class ScheduleRuleAttributesEntityConfiguration : IEntityTypeConfiguration<ScheduleRuleAttribute>
    {
        public void Configure(EntityTypeBuilder<ScheduleRuleAttribute> builder)
        {
            builder.ToTable("ScheduleRuleAttribute");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.ScheduleRule)
            .WithMany(m => m.RuleAttributes)
            .HasForeignKey("RuleId");
        }
    }
}
