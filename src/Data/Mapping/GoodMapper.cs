using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodMapper
{
    public static void MapGoods(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Good>(entity =>
        {
            entity
                .HasKey(e => e.Id);

            entity
                .HasOne(x => x.Coin);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .VarcharWithMaxLength(250);

            entity
                .HasOne(e => e.GoodType);
        });
    }
}