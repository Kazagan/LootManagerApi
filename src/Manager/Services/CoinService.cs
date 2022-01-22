using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Services;

public class CoinService
{
    private readonly IRepository<ManagerContext> _repository;

    public CoinService(IRepository<ManagerContext> repository)
    {
        _repository = repository;
    }

    public Coin Get(int treasureLevel, int roll)
    {
        var coinRoll = GetCoin(treasureLevel, roll);

        if (coinRoll?.Coin is null)
            return new Coin();

        var output = coinRoll.Coin;
        output.Count = SharedFunctions.GetValue(coinRoll.DiceCount, coinRoll.DiceSides, coinRoll.Multiplier);
        return output;
    }

    private CoinRoller? GetCoin(int treasureLevel, int roll)
    {
        var x =  _repository
            .Get<CoinRoller>()
            .Include(x => x.Coin)
            .FirstOrDefault(x => 
                x.TreasureLevel == treasureLevel 
                && roll <= x.RollMax 
                && roll >= x.RollMin);
        return x;
    }
}