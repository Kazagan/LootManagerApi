using Manager.Enums;

namespace Manager.Models;

public class CoinTable
{
    public CoinTable(int treasureLevel, int min, int max, Coin coin, int diceCount, int diceSides, int multiplier)
    {

        Min = min;
        Max = max;
        Coin = coin;
        DiceCount = diceCount;
        DiceSides = diceSides;
        Multiplier = multiplier;
        TreasureLevel = treasureLevel;
    }

    public int TreasureLevel { get; }
    public int Min { get; }
    public int Max { get; }
    public Coin Coin { get; }
    public int DiceCount { get; }
    public int DiceSides { get; }
    public int Multiplier { get; }

    public bool InRange(int x) => x <= Max && x >= Min;
}

public static class CoinTableRows // TODO Seed in Database
{
    public static List<CoinTable> GetCoinTable()
    {
        return new List<CoinTable>
        {
            new(1, 1, 14, Coins.Copper, 2, 6, 1000),
            new(1, 15, 29, Coins.Nickel, 4, 6, 100),
            new(1, 30, 52, Coins.Silver, 2, 6, 100),
            new(1, 53, 95, Coins.Gold, 2, 8, 10),
            new(1, 96, 100, Coins.Platinum, 1, 4, 10)
        };
    }
}