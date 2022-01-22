namespace Data.Models;

public class CoinRoller
{
    public int Id { get; set; }
    public int TreasureLevel { get; set; }
    public int RollMin { get; set; }
    public int RollMax { get; set; }
    public Coin? Coin { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }
}