using System.Reflection;
using Data.Entities;
using Data.Repositories;

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
    public IEnumerable<CoinRoller> GetAll() => _repository.Get<CoinRoller>();
    public CoinRoller? Get(Guid id) => _repository.Get<CoinRoller>(id);

    public CoinRoller? Get(int treasureLevel, int roll)
    {
        return GetAll()
            .Where(x => x.TreasureLevel == treasureLevel)
            .OrderBy(x => x.RollMin)
            .LastOrDefault(x => x.RollMin < roll);
    }

    public CoinRoller Create(CoinRoller coinRoller)
    {
        if (Exists(coinRoller))
        {
            throw new Exception("Roller already exists for this treasure level and minimum");
        }

        var coin = GetCoin(coinRoller.Coin);
        if (coin is null)
        {
            throw new Exception("No Coin found for input.");
        }

        coinRoller.Coin = coin;
        _repository.Insert(coinRoller);
        _repository.Save();
        return coinRoller;
    }

    private Coin? GetCoin(Coin coin)
    {
        if (coin.Id != Guid.Empty)
        {
            return _coinService.Get(coin.Id);
        }

        return !string.IsNullOrEmpty(coin.Name) ? _coinService.Get(coin.Name) : null;
    }

    private bool Exists(CoinRoller coinRoller)
    {
        return GetRoll(coinRoller.TreasureLevel, coinRoller.RollMin) is not null;
    }

    // Get for specific treasure level and roll, rather than the next, used for ensuring roll is not already set.
    private CoinRoller? GetRoll(int treasureLevel, int rollMin) => 
        GetAll().FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.RollMin == rollMin);
}