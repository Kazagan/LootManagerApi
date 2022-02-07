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
                .HasOne(x => x.Value);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

            entity
                .HasOne(e => e.GoodType);
        });
    }
}