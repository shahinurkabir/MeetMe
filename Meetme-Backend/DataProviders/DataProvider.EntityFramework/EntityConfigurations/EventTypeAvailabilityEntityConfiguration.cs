using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class EventTypeAvailabilityDetailConfiguration : IEntityTypeConfiguration<EventTypeAvailabilityDetail>
    {
        public void Configure(EntityTypeBuilder<EventTypeAvailabilityDetail> builder)
        {
            builder.ToTable("EventTypeAvailabilityDetails"); // Set the table name
            builder.HasKey(ead => ead.Id); // Set the primary key

            // Configure properties
            builder.Property(ead => ead.Id)
                .IsRequired()
                .HasColumnType("INTEGER");

            builder.Property(ead => ead.EventTypeId)
                .HasColumnType("uniqueidentifier");

            builder.Property(ead => ead.DayType)
                .IsRequired()
                .HasColumnType("varchar(15)");

            builder.Property(ead => ead.Value)
                .IsRequired()
                .HasColumnType("varchar(15)");

            builder.Property(ead => ead.StepId)
                .HasColumnType("smallint");
            
            builder.Property(ead => ead.From)
                .HasColumnType("float");

            builder.Property(ead => ead.To)
                .HasColumnType("float");

            // Configure foreign key relationship
            builder.HasOne(ead => ead.EventType)
                   .WithMany(et => et.EventTypeAvailabilityDetails)
                   .HasForeignKey(ead => ead.EventTypeId);
                   //.OnDelete(DeleteBehavior.Restrict);

        }
    }


}
