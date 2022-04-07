namespace Manager;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHost(args)
            .Build()
            .Run();
    }

    private static IHostBuilder CreateHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}