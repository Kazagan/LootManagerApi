using Data.Models;
using Data.Repositories;

namespace Manager.Services;

public class CoinService
{
    private readonly Random _random;
    private readonly IRepository<ManagerContext> _repository;

    public CoinService(IRepository<ManagerContext> repository)
    {
        _random = new Random();
        _repository = repository;
    }

    public Coin? Get(int treasureLevel, int roll)
    {
        var coinRoll = _repository.Get<CoinTable>()
            .FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.InRange(roll));

        if (coinRoll is null) // TODO set up Logger, or return that something went wrong, throw error?
            return null;

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