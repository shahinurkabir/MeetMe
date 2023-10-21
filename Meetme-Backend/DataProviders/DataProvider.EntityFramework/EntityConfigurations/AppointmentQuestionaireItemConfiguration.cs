using MeetMe.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class AppointmentQuestionaireItemConfiguration : IEntityTypeConfiguration<AppointmentQuestionaireItem>
{
    public void Configure(EntityTypeBuilder<AppointmentQuestionaireItem> builder)
    {
        builder.ToTable("AppointmentQuestionaireItems"); // Set the table name
        builder.HasKey(a => a.Id); // Set the primary key

        // Configure properties

        builder.Property(a => a.QuestionName)
               .IsRequired()
               .HasColumnType("varchar(max)");

        builder.Property(a => a.Answer)
               .IsRequired()
               .HasColumnType("varchar(255)");


    }

}