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
        var coinRoll = CoinTableRows.GetCoinTable();
        var x = coinRoll.FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.InRange(roll));

        if (x is null) // TODO set up Logger, or return that something went wrong, throw error?
            return new Coin(0.0, CoinType.Iron);

        var output = x.Coin;
        output.Count = GetCount(x.DiceCount, x.DiceSides, x.Multiplier);
        return output;
    }

    private int GetCount(int diceCount, int diceSides, int multiplier)
    {
        var roll = 0;
        for (var i = 0; i < diceCount; i++)
            roll += _random.Next(1, diceSides);
        return roll *= multiplier;
    }
}