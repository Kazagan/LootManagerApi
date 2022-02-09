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

    public Coin Create(Coin coin)
    {
        if (string.IsNullOrEmpty(coin.Name) || coin.InGold is null)
        {
            return new Coin {Id = -1};
        }
        if (NameTaken(coin.Name))
        {
            return new Coin {Id = 0};
        }
        var newCoin = new Coin {Name = coin.Name, InGold = coin.InGold};
        _repository.Insert(newCoin);
        _repository.Save();
        return newCoin;
    }

    public Coin Update(Coin coin)
    {
        var original = Get(coin.Id);
        if (original is null)
        {
            return new Coin {Id = -1};
        }
        
        if (IsNewNameAndValid(coin, original) )
        {
            return new Coin {Id = 0};
        }

        original.Name = coin.Name ?? original.Name;
        original.InGold = coin.InGold ?? original.InGold;
        _repository.Update(original);
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
    

    private bool IsNewNameAndValid(Coin coin, Coin original)
    {
        return original.Name != coin.Name 
               && !string.IsNullOrEmpty(coin.Name) 
               && NameTaken(coin.Name);
    }
}