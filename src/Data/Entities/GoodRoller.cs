namespace Data.Entities;

public class GoodRoller : Entity
{
    GoodRoller()
    {
        Good = new Good();
    }
    public int RollMin { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }
    public Good Good { get; set; }
    public override bool IsInvalid()
    {
        throw new NotImplementedException();
    }
}