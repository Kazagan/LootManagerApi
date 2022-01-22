using Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodTypeRollerMapper
{
    public static void MapGoodType(this ModelBuilder modelBuilder)
    {
        var typeLength = SharedFunctions.GetEnumMaxLength<GoodType>();
        modelBuilder.Entity<Models.GoodTypeRoller>(entity =>
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(typeLength);
            
            entity
                .Property(e => e.TreasureLevel)
                .IsRequired(); 
            entity
                .Property(e => e.RollMin)
                .IsRequired();
            entity
                .Property(e => e.RollMax)
                .IsRequired();
        });
    }
}