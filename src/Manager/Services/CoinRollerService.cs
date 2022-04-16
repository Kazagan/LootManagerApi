using Data;
using Data.Entities;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Services;

public class CoinRollerService
{
    private readonly IRepository _repository;
    private readonly CoinService _coinService;

    public CoinRollerService(IRepository repository)
    {
        _repository = repository;
        _coinService = new CoinService(_repository);
    }
    public IEnumerable<CoinRoller> Get() => _repository.Get<CoinRoller>()
        .Include(x => x.Coin);
    public CoinRoller? Get(Guid id) => Get().FirstOrDefault(x => x.Id == id);

    public IEnumerable<CoinRoller> Get(int treasureLevel) =>
        Get().Where(x => x.TreasureLevel == treasureLevel);

    public CoinRoller? Get(int treasureLevel, int roll)
    {
        return Get(treasureLevel)
            .OrderBy(x => x.RollMin)
            .LastOrDefault(x => x.RollMin < roll);
    }

    public string Create(CoinRoller coinRoller)
    {
        if (Exists(coinRoller))
            return Constants.Exists;
        var coin = _coinService.Get(coinRoller.Coin);
        if (coin is null)
            return "Must provide existing coin.";
        if (coinRoller.IsInvalid())
            return Constants.Invalid;

        coinRoller.Coin = coin;
        _repository.Insert(coinRoller);
        _repository.Save();
        return Constants.Success;
    }

    public string Update(CoinRoller roller)
    {
        var original = Get(roller.Id);
        if (original is null)
        {
            return Constants.NotFound;
        }

        if (Changed(original, roller) && Exists(roller))
        {
            return Constants.Exists;
        }

        if (CoinChanged(original, roller))
        {
            var coin = _coinService.Get(roller.Coin);
            if (coin is null)
            {
                return "New coin not found";
            }
            original.Coin = coin;
        }

        original.Copy(roller);
        _repository.Update(original);
        _repository.Save();
        return Constants.Success;
    }

    public bool Delete(Guid id)
    {
        var roller = Get(id);
        if (roller is null)
        {
            return false;
        }
        _repository.Delete(roller);
        _repository.Save();
        return true;
    }

    private bool CoinChanged(CoinRoller original, CoinRoller roller)
    {
        return (roller.Coin.Id != Guid.Empty && original.Coin.Id != roller.Coin.Id) ||
               (!string.IsNullOrEmpty(roller.Coin.Name) && original.Coin.Name != roller.Coin.Name);
    }

    private bool Changed(CoinRoller original, CoinRoller roller)
    {
        return original.TreasureLevel != roller.TreasureLevel &&
               original.RollMin != roller.RollMin;
    }

    private bool Exists(CoinRoller roller)
    {
        return GetRoll(roller.TreasureLevel, roller.RollMin) is not null;
    }



    // Get for specific treasure level and roll, rather than the next, used for ensuring roll is not already set.
    private CoinRoller? GetRoll(int treasureLevel, int rollMin) =>
        Get().FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.RollMin == rollMin);
}