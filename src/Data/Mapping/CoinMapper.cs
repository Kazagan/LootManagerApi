using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class CoinMapper
{
    public static void MapCoins(this ModelBuilder modelBuilder)
    {
        var length = SharedFunctions.GetEnumMaxLength<CoinType>();
        modelBuilder.Entity<Coin>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity
                .Property(e => e.CoinType)
                .HasConversion<string>()
                .HasMaxLength(length)
                .IsRequired();
            entity
                .Property(e => e.InGold)
                .IsRequired()
                .HasPrecision(4, 4);
            entity
                .Ignore(e => e.Count);

            entity
                .HasData(Enum
                    .GetValues<CoinType>()
                    .Select(x => new Coin()
                    {
                        CoinType = x
                    }));
        });
    }
}