using Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ManagerContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=host.docker.internal,1433;Database=LootTracker;User Id=SA;Password=Password#123;TrustServerCertificate=true");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapCoins();
        modelBuilder.MapCoinTable();
        modelBuilder.MapGoods();
        modelBuilder.MapGoodRoller();
        modelBuilder.MapGoodType();
    }
}