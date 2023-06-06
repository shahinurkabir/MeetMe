using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    //public class EventTypeAvailabilityDetailsEntityConfiguration : IEntityTypeConfiguration<EventTypeAvailabilityDetail>
    //{
    //    public void Configure(EntityTypeBuilder<EventTypeAvailabilityDetail> builder)
    //    {
    //        builder.ToTable("EventTypeAvailabilityDetail");
    //        builder.HasKey(e => e.Id);
    //        builder.HasOne(e => e.EventType)
    //            .WithMany(e => e.EventTypeAvailabilityDetails)
    //            .HasForeignKey(f => f.EventTypeId)
    //            .OnDelete(DeleteBehavior.SetNull);
    //    }
    //}
}
