using Data.Enums;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Manager.Services;

public class GoodService
{
    private readonly Random _random;
    private readonly IRepository<ManagerContext> _repository;

    public GoodService(IRepository<ManagerContext> repository)
    {
        _random = new Random();
        _repository = repository;
    }
    
    public Good Get(int treasureLevel, int roll)
    {
        var type = GetGoodType(treasureLevel, roll);
        var rolledGood = GetGood(_random.Next(1, 100), type);

        if (rolledGood is null)
            return new Good();

        var output = rolledGood.Good;
        output.Value.Count = SharedFunctions.GetValue(rolledGood.DiceCount, rolledGood.DiceSides, rolledGood.Multiplier);
        return output;
    }

    private GoodRoller? GetGood(int roll, GoodType type)
    {
        return _repository
            .Get<GoodRoller>()
            .Include(x => x.Good)
            .Include(x => x.Good.Value)
            .FirstOrDefault(x => 
                roll >= x.RollMin 
                && roll <= x.RollMax
                && type == x.Good.Type);
    }

    private GoodType GetGoodType(int treasureLevel, int roll)
    {
        var goodType = _repository
            .Get<GoodTypeRoller>()
            .FirstOrDefault(x => 
                x.TreasureLevel == treasureLevel 
                && roll >= x.RollMin 
                && roll <= x.RollMax);
        
        return goodType?.Type ?? GoodType.SmallGem;
    }
}