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
    public IEnumerable<CoinRoller> GetAll() => _repository.Get<CoinRoller>()
        .Include(x => x.Coin);
    public CoinRoller? Get(Guid id) => GetAll().FirstOrDefault(x => x.Id == id);

    public CoinRoller? Get(int treasureLevel, int roll)
    {
        return GetAll()
            .Where(x => x.TreasureLevel == treasureLevel)
            .OrderBy(x => x.RollMin)
            .LastOrDefault(x => x.RollMin < roll);
    }

    public string Create(CoinRoller coinRoller)
    {
        if (Exists(coinRoller))
        {
            return "Roller already exists for this treasure level and minimum";
        }

        var coin = _coinService.Get(coinRoller.Coin);
        if (coin is null)
        {
            return "Must provide existing coin.";
        }

        coinRoller.Coin = coin;
        _repository.Insert(coinRoller);
        _repository.Save();
        return Constants.Success;
    }

    private bool Exists(CoinRoller coinRoller)
    {
        return GetRoll(coinRoller.TreasureLevel, coinRoller.RollMin) is not null;
    }
    
    

    // Get for specific treasure level and roll, rather than the next, used for ensuring roll is not already set.
    private CoinRoller? GetRoll(int treasureLevel, int rollMin) => 
        GetAll().FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.RollMin == rollMin);
}