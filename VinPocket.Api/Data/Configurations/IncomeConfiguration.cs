using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinPocket.Api.Models;

namespace VinPocket.Api.Data.Configurations;

public sealed class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Source)
            .HasMaxLength(100);

        builder.HasOne(x =>x .User)
          .WithMany()
          .HasForeignKey(x => x.UserId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
