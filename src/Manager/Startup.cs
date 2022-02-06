using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager;

public class Startup
{
    private IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private void ConfigureServices(IServiceCollection services)
    {

        services.AddControllers();
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
        
        services.BindServices();
        services.AddDbContext<ManagerContext>(
            options => options.UseSqlServer(
                _configuration.GetConnectionString("manager"),
                builder => builder.SqlOptions()
            ));
    }
}