using Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ManagerContext : DbContext
{
    public ManagerContext(DbContextOptions<ManagerContext> options) : base(options)
    {
        
    }

    public ManagerContext() : base()
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapCoins();
        modelBuilder.MapCoinTable();
        modelBuilder.MapGoods();
        modelBuilder.MapGoodRoller();
        modelBuilder.MapGoodTypeRoller();
        modelBuilder.MapGoodType();
    }
}