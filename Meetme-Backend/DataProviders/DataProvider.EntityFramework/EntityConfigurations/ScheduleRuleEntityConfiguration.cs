using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class ScheduleRuleEntityConfiguration : IEntityTypeConfiguration<ScheduleRule>
    {
        public void Configure(EntityTypeBuilder<ScheduleRule> builder)
        {
            builder.ToTable("ScheduleRule");
            builder.HasKey(e => e.Id);
        }
    }
}
