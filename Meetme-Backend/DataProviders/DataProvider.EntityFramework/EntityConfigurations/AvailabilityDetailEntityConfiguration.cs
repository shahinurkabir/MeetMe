using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class AvailabilityDetailEntityConfiguration : IEntityTypeConfiguration<AvailabilityDetail>
    {
        public void Configure(EntityTypeBuilder<AvailabilityDetail> builder)
        {
            builder.ToTable("AvailabilityDetail");
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Availability)
            .WithMany(m => m.Details)
            .HasForeignKey("RuleId");
        }
    }
}
