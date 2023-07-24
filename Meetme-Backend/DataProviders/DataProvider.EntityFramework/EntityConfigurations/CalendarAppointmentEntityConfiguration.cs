using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeetMe.Core.Persistence.Entities;

namespace DataProvider.EntityFramework.EntityConfigurations
{
    public class CalendarAppointmentEntityConfiguration : IEntityTypeConfiguration<CalendarAppointment>
    {
        public void Configure(EntityTypeBuilder<CalendarAppointment> builder)
        {
            builder.ToTable("CalendarAppointment");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(e => e.EventType)
            .WithMany(m => m.Appointments)
            .HasForeignKey("EventTypeId");
        }
    }
}
