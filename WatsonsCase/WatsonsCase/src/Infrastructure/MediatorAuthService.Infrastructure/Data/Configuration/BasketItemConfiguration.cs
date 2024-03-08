using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatsonsCase.Domain.Entities;

namespace WatsonsCase.Infrastructure.Data.Configuration;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.Property(x => x.ProductName)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(x => x.Quantity)
            .IsRequired();
        builder.Property(x => x.Price)
            .IsRequired().HasColumnType("decimal(18,2)");
    }
}