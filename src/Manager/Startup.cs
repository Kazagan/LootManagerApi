using Data.Repositories;
using Manager.Services;
using Microsoft.EntityFrameworkCore;

namespace Manager;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.BindRepositories();
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();

        // services.BindServices();
        services.AddHttpClient();

        services.AddDbContext<ManagerContext>(
            options => options.UseNpgsql(
                _configuration.GetConnectionString("manager"),
                builder => builder.EnableRetryOnFailure().CommandTimeout(120)
            ));
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAuthentication();
        if (env.IsDevelopment() || env.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase))
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/", "Manager V1"));
        }

        app.UseRouting();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        DatabaseService.MigrationInitialisation(app);
    }
}