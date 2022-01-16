using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public Coin Get(int treasureLevel, int roll)
    {
        var coinRoll = _repository
            .Get<CoinTable>()
            .Include(x => x.Coin)
            .FirstOrDefault(x => x.TreasureLevel == treasureLevel && roll <= x.Max && roll >= x.Min);

        if (coinRoll?.Coin is null) // TODO set up Logger, or return that something went wrong, throw error?
            return new Coin();

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