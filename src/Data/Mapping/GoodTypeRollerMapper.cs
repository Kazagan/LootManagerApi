using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodTypeRollerMapper
{
    public static void MapGoodTypeRoller(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodTypeRoller>(entity =>
        {
            entity
                .HasKey(e => e.Id);

            entity
                .HasOne(e => e.GoodType);

            entity
                .Property(e => e.TreasureLevel)
                .IsRequired(); 
            entity
                .Property(e => e.RollMin)
                .IsRequired();
        });
    }
}