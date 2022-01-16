using Data.Repositories;
using Manager.Services;

namespace Manager;

public static class Startup
{
    public static void Run()
    {
        var repo = new Repository<ManagerContext>(new ManagerContext());
        var coinService = new CoinService(repo);
        var roller = new Roller(coinService);

        var input = "";
        while (input != "exit")
        {
            input = Console.ReadLine();
            if (!int.TryParse(input, out var treasureLevel))
            {
                Console.WriteLine("Input was not a number, try again");
            }

            var result = roller.Roll(treasureLevel);
            Console.WriteLine($"Cash Roll: {result.Cash.Count} {result.Cash.CoinType}");
        }
    }
}