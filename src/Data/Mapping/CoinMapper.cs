using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class CoinMapper
{
    public static void MapCoins(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coin>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            
            entity
                .Property(e => e.Name)
                .IsRequired()
                .VarcharWithMaxLength(250);
            
            entity
                .Property(e => e.InGold)
                .IsRequired()
                .HasPrecision(10, 4);
        });
    }
}