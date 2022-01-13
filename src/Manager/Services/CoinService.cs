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
        return roll switch
        {
            < 15 => new Coin(0),
            < 30 => Coins.Copper,
            < 53 => Coins.Silver,
            < 96 => Coins.Gold,
            _ => Coins.Platinum
        };
    }
}