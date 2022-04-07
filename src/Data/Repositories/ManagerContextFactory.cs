using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Repositories;

public class ManagerContextFactory : IDesignTimeDbContextFactory<ManagerContext>
{
    public ManagerContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ManagerContext>();
        options.UseNpgsql("Host=db;Username=lootManager;Password=Password#123;Database=lootManager_db");

        return new ManagerContext(options.Options);
    }
}