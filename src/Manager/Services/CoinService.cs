using Manager.Models;

namespace Manager.Services;

public class CoinService
{
    private readonly Random _random;

    public CoinService()
    {
        _random = new Random();
    }

    public Coin? Get(int treasureLevel, int roll)
    {
        if (roll < 15)
            return null;
        if(roll < 30)
            return Coins.Copper;
        if(roll < 53)
            return Coins.Silver;
        return Coins.Gold;
    }
}