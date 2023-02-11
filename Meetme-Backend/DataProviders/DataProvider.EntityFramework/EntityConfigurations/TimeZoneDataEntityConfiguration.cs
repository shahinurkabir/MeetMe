using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class TimeZoneDataEntityConfiguration : IEntityTypeConfiguration<TimeZoneData>
    {
        public void Configure(EntityTypeBuilder<TimeZoneData> builder)
        {
            builder.ToTable("TimeZoneData");
            builder.HasKey(e => e.Id);
            builder.Property(b => b.Name).IsRequired();
        }
    }

}
