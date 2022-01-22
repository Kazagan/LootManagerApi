using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodRollerMapping
{
    public static void MapGoodRoller(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodRoller>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .HasOne(e => e.Good);

            entity
                .Property(e => e.RollMin)
                .IsRequired();
            entity
                .Property(e => e.RollMax)
                .IsRequired();
            entity
                .Property(e => e.DiceCount)
                .IsRequired();
            entity
                .Property(e => e.DiceSides)
                .IsRequired();
            entity
                .Property(e => e.Multiplier)
                .IsRequired();
        });
    }
}