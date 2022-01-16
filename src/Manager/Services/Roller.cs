using System.Runtime.InteropServices;
using Data.Models;
using Manager.Models;

namespace Manager.Services;

public class Roller
{
    private readonly Random _random;

    public Roller()
    {
        _random = new Random();
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
        var coins = new Coin {};
        return coins;
    }
}