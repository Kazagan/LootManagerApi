using Data.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Manager;

public static class Dependency
{
    public static void BindRepositories(this IServiceCollection service)
    {
        service.AddScoped<IRepository, Repository<ManagerContext>>();
    }
}