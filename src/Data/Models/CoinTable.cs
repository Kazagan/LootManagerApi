namespace Data.Models;

public class CoinTable
{
    public int Id { get; set; }
    public int TreasureLevel { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public Coin? Coin { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }
}