using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seb.Server.Domain;

namespace Seb.Server.Infrastructure.Persistence.EntityTypeConfigurations;

public class CurrencyDataConfiguration : IEntityTypeConfiguration<CurrencyData>
{
    public void Configure(EntityTypeBuilder<CurrencyData> builder)
    {
        builder.ToTable("CurrencyData");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.DateStamp);

        builder.OwnsMany(x => x.Currencies, ob =>
        {
            ob.ToTable("Currencies");
            ob.Property(c => c.Code).HasMaxLength(Currency.CodeMaxLength);
            ob.Property(c => c.Name).HasMaxLength(Currency.NameMaxLength);
            ob.Property(c => c.Rate).HasColumnType("decimal(18, 4)");
        });
       
    }
}