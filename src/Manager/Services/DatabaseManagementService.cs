using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Services;

public static class DatabaseService
{
    public static void MigrationInitialisation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        serviceScope.ServiceProvider.GetService<ManagerContext>()?.Database.Migrate();
    }

}