using System.Reflection;
using Data.Entities;
using Data.Repositories;

namespace Manager.Services;

public class CoinRollerService
{
    private readonly IRepository<ManagerContext> _repository;

    public CoinRollerService(IRepository<ManagerContext> repository)
    {
        _repository = repository;
    }
    public IEnumerable<CoinRoller> GetAll() => _repository.Get<CoinRoller>();
    public CoinRoller? Get(int id) => _repository.Get<CoinRoller>(id);

    public CoinRoller? Get(int treasureLevel, int roll)
    {
        return GetAll()
            .Where(x => x.TreasureLevel == treasureLevel)
            .OrderBy(x => x.RollMin)
            .LastOrDefault(x => x.RollMin < roll);
    }

    public CoinRoller Create(CoinRoller coinRoller)
    {
        if (CheckRoller(coinRoller))
        {
            return new CoinRoller {Id = -1};
        }

        coinRoller.Coin = _repository.Get<Coin>(coinRoller.Coin.Id);
        _repository.Insert(coinRoller);
        _repository.Save();
        return coinRoller;
    }

    private bool CheckRoller(CoinRoller coinRoller)
    {
        return GetRoll(coinRoller.TreasureLevel, coinRoller.RollMin) is not null;
    }

    // Get for specific treasure level and roll, rather than the next, used for ensuring roll is not already set.
    private CoinRoller? GetRoll(int treasureLevel, int rollMin) => 
        GetAll().FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.RollMin == rollMin);
}