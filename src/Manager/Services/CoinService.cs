using System.Runtime.CompilerServices;
using Data.Entities;
using Data.Repositories;

namespace Manager.Services;

public class CoinService
{
    private readonly IRepository _repository;

    public CoinService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Coin> Get() => _repository.Get<Coin>();

    public Coin? Get(Guid id) => _repository.Get<Coin>(id);

    public Coin? Get(string name)
    {
        return _repository.Get<Coin>()
            .FirstOrDefault(x => x.Name == name);
    }

    public Coin? Get(Coin coin)
    {
        if (coin.Id != Guid.Empty)
        {
            return Get(coin.Id);
        }

        return string.IsNullOrEmpty(coin.Name) ? null : Get(coin.Name);
    }

    public string Create(Coin coin)
    {
        if (NameIsTaken(coin.Name))
        {
            return "Coin Name taken";
        }
        _repository.Insert(coin);
        _repository.Save();
        return Constants.Success;
    }

    public string Update(Coin coin)
    {
        var original = Get(coin);
        if (original is null)
        {
            return Constants.NotFound;
        }
        
        if (IsNewNameAndValid(coin, original) )
        {
            return "Coin name already exists, or is empty";
        }
        
        _repository.Update(coin);
        _repository.Save();
        return Constants.Success;
    }

    public bool Delete(Guid id)
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
    
    private bool NameIsTaken(string name)
    {
        return Get(name) is not null;
    }
    

    private bool IsNewNameAndValid(Coin coin, Coin original)
    {
        return original.Name != coin.Name 
               && !string.IsNullOrEmpty(coin.Name) 
               && NameIsTaken(coin.Name);
    }
}