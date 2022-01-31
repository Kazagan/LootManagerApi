using System.Linq;
using Data.Entities;
using Data.Enums;
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
    
    public Good Get(int treasureLevel, int roll, int goodRoll)
    {
        var type = GetGoodType(treasureLevel, roll);
        var rolledGood = GetGood(goodRoll, type);

        if (rolledGood is null)
            return new Good();

        var output = rolledGood.Good;
        output.Value.Count = SharedFunctions.GetValue(rolledGood.DiceCount, rolledGood.DiceSides, rolledGood.Multiplier);
        return output;
    }

    private GoodRoller? GetGood(int roll, GoodType type)
    {
        var goodRollers = _repository
            .Get<GoodRoller>();
        var x =  goodRollers
            .Include(x => x.Good)
            .Include(x => x.Good.Value)
            .LastOrDefault(x => 
                roll >= x.RollMin
                && type == x.Good.Type);
        return x;
    }

    private GoodType GetGoodType(int treasureLevel, int roll)
    {
        var goodType = _repository
            .Get<GoodTypeRoller>()
            .LastOrDefault(x => 
                x.TreasureLevel == treasureLevel 
                && roll >= x.RollMin);

        return goodType?.Type ?? GoodType.Art;
    }
}