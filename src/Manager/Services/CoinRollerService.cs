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
        
    }

    private CoinRoller? GetRoll(int treasureLevel, int rollMin) => 
        GetAll().FirstOrDefault(x => x.TreasureLevel == treasureLevel && x.RollMin == rollMin);
}