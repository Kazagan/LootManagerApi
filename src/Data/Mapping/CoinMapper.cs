using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class CoinMapper
{
    public static void MapCoins(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coin>(entity =>
        {
            entity.HasKey(e => e.CoinType);
            entity.Property(e => e.CoinType).HasConversion<string>().IsRequired();
            entity.Property(e => e.InGold).IsRequired().HasPrecision(4, 4);
            entity.Ignore(e => e.Count);
        });
    }
}