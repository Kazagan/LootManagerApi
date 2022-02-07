using System.Diagnostics;
using System.Runtime.CompilerServices;
using Data.Entities;
using Data.Repositories;

namespace Manager.Services;

public class CoinService
{
    private IRepository<ManagerContext> _repository;

    public CoinService(IRepository<ManagerContext> repository)
    {
        _repository = repository;
    }

    public IEnumerable<Coin> GetAll()
    {
        return _repository.Get<Coin>();
    }

    public Coin? Get(int id)
    {
        return _repository
            .Get<Coin>()
            .FirstOrDefault(x => x.Id == id);
    }

    public Coin? Get(string name)
    {
        return _repository
            .Get<Coin>()
            .FirstOrDefault(x => x.Name == name);
    }

    public Coin? Create(string name, decimal inGold)
    {
        if (NameTaken(name))
        {
            return null;
        }
        var newCoin = new Coin {Name = name, InGold = inGold};
        _repository.Insert(newCoin);
        _repository.Save();
        return newCoin;
    }

    public Coin Update(int id, string? name, decimal? inGold)
    {
        var coin = Get(id);
        if (coin is null)
        {
            return new Coin {Id = -1};
        }
        if (!coin.Name.Equals(name, StringComparison.Ordinal) && !string.IsNullOrEmpty(name) && NameTaken(name ?? ""))
        {
            return new Coin {Id = 0};
        }
        coin.Name = name ?? coin.Name;
        coin.InGold = inGold ?? coin.InGold;
        _repository.Update(coin);
        _repository.Save();
        return coin;
    }

    public bool Delete(int id)
    {
        var coin = Get(id);
        if (coin is null)
        {
            return false;
        }
        _repository.Delete(coin);
        _repository.Save();
        return true;
    }
    
    private bool NameTaken(string name)
    {
        return Get(name) is not null;
    }
}