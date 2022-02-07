using Data.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Manager;

public static class Dependency
{
    public static void BindRepositories(this IServiceCollection service)
    {
        service.AddScoped<IRepository<ManagerContext>, Repository<ManagerContext>>();
    }
    public static void BindServices(this IServiceCollection services)
    {
    }

    public static void SqlOptions(this SqlServerDbContextOptionsBuilder options)
    {
        options.EnableRetryOnFailure();
    }
}