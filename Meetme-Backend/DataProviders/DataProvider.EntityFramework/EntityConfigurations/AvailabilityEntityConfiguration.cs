using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class AvailabilityEntityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.ToTable("Availability");
            builder.HasKey(e => e.Id);
        }
    }
}
