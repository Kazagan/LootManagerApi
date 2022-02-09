namespace Data.Entities;

public class CoinRoller : Entity
{
    public int? TreasureLevel { get; set; }
    public int? RollMin { get; set; }
    public Coin? Coin { get; set; }
    public int? DiceCount { get; set; }
    public int? DiceSides { get; set; }
    public int? Multiplier { get; set; }
}