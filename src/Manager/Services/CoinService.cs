using Manager.Enums;
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
        var coinRoll = CoinTableRows
            .GetCoinTable()
            .FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.InRange(roll));

        if (coinRoll is null) // TODO set up Logger, or return that something went wrong, throw error?
            return new Coin(0.0, CoinType.Iron);

        var output = coinRoll.Coin;
        output.Count = GetCount(coinRoll.DiceCount, coinRoll.DiceSides, coinRoll.Multiplier);
        return output;
    }

    private int GetCount(int diceCount, int diceSides, int multiplier)
    {
        var roll = 0;
        for (var i = 0; i < diceCount; i++)
            roll += _random.Next(1, diceSides);
        return roll * multiplier;
    }
}