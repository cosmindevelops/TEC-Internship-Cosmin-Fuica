using Internship.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiApp.Data.Config;

public class PersonDetailsConfiguration : IEntityTypeConfiguration<PersonDetails>
{
    public void Configure(EntityTypeBuilder<PersonDetails> builder)
    {
        // Configure primary key
        builder.HasKey(pd => pd.Id);

        // Configure properties
        builder.Property(pd => pd.BirthDay)
            .IsRequired();

        builder.Property(pd => pd.PersonCity)
                .IsRequired();

        builder.HasOne(pd => pd.Person)
                .WithOne(p => p.PersonDetails)
                .HasForeignKey<PersonDetails>(pd => pd.PersonId);
    }
}