using Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.SQLServer.Mappings;
public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(u => u.FullName)
            .IsRequired() // NOT NULL
            .IsUnicode(false) // VARCHAR
            .HasMaxLength(100);

        builder.OwnsOne(customer => customer.Email, ownedNav =>
        {
            ownedNav
                .Property(email => email.Address)
                .IsRequired() // NOT NULL
                .IsUnicode(false) // VARCHAR
                .HasMaxLength(254)
                .HasColumnName(nameof(User.Email));

            // Unique Index
            ownedNav
                .HasIndex(email => email.Address)
                .IsUnique();
        });

        builder
           .Property(u => u.BirthDate)
           .IsRequired() // NOT NULL
           .HasColumnType("DATE");
    }
}
