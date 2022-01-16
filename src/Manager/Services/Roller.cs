using System.Runtime.InteropServices;
using Data.Models;
using Data.Repositories;
using Manager.Models;

namespace Manager.Services;

public class Roller
{
    private readonly Random _random;
    private readonly CoinService _coinService;

    public Roller(CoinService coinService)
    {
        _random = new Random();
        _coinService = coinService;
    }
    public RollerResult Roll(int treasureLevel)
    {
        return new RollerResult()
        {
            Cash = RollGold(treasureLevel, _random.Next(1, 100)),
            Gems = _random.Next(1, 100),
            Items = _random.Next(1, 100)
        };
    }

    private Coin RollGold(int treasureLevel, int roll)
    {
        return _coinService.Get(treasureLevel, roll);
    }
}