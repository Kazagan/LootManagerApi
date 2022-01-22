namespace Data.Models;

public class GoodRoller
{
    public int Id { get; set; }
    public int RollMin { get; set; }
    public int RollMax { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }
    public Good Good { get; set; }
}