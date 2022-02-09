namespace Data.Entities;

public class GoodRoller : Entity
{
    public int RollMin { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }
    public Good Good { get; set; }
}