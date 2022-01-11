using Manager.Models;

namespace Manager.Services;

public class CoinService
{
    private readonly Random _random;

    public CoinService()
    {
        _random = new Random();
    }

    public Coin Get(int treasureLevel, int roll)
    {
        return Coins.Copper;
    }
}