using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Repositories;

public class ManagerContextFactory : IDesignTimeDbContextFactory<ManagerContext>
{
    public ManagerContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ManagerContext>();
        options.UseSqlServer("Server=host.docker.internal,1433;Database=LootTracker;User Id=SA;Password=Password#123;TrustServerCertificate=true");

        return new ManagerContext(options.Options);
    }
}