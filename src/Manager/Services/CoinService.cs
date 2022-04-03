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

    public IEnumerable<Coin> GetAll() => _repository.Get<Coin>();

    public Coin? Get(Guid id) => _repository.Get<Coin>(id);

    public Coin? Get(string name)
    {
        return _repository
            .Get<Coin>()
            .FirstOrDefault(x => x.Name == name);
    }

    private Coin? Get(Coin coin)
    {
        if (coin.Id != Guid.Empty)
        {
            return Get(coin.Id);
        }
        return !string.IsNullOrEmpty(coin.Name) ? Get(coin.Name) : null;
    }

    public Coin Create(Coin coin)
    {
        if (string.IsNullOrEmpty(coin.Name) || coin.InGold == 0)
        {
            throw new Exception("Name or in Gold Value not set");
        }
        if (NameIsTaken(coin.Name))
        {
            throw new Exception("Coin Name taken");
        }
        var newCoin = new Coin {Name = coin.Name, InGold = coin.InGold};
        _repository.Insert(newCoin);
        _repository.Save();
        return newCoin;
    }

    public Coin Update(Coin coin)
    {
        var original = Get(coin);
        if (original is null)
        {
            throw new Exception("Coin is Null");
        }
        
        if (IsNewNameAndValid(coin, original) )
        {
            throw new Exception("Coin name already exists, or is empty");
        }

        original.Name = string.IsNullOrEmpty(coin.Name) ? original.Name : coin.Name;
        original.InGold = coin.InGold == 0 ? original.InGold : coin.InGold;
        _repository.Update(original);
        _repository.Save();
        return original;
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