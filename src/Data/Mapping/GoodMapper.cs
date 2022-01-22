using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodMapper
{
    public static void MapGoods(this ModelBuilder modelBuilder)
    {
        var length = SharedFunctions.GetEnumMaxLength<GoodType>();
        
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
                .Property(e => e.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(length);
        });
    }
}