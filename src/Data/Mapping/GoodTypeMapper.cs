using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Mapping;

public static class GoodTypeMapper
{
    public static void MapGoodType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoodType>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Name)
                .VarcharWithMaxLength(250)
                .IsRequired();
        });
    }
}