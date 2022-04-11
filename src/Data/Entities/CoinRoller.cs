namespace Data.Entities;

public class CoinRoller : Entity
{
    public int TreasureLevel { get; set; }
    public int RollMin { get; set; }
    public Coin Coin { get; set; } = new ();
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }

    public virtual bool IsInvalid() =>
        TreasureLevel == 0 || RollMin == 0 || DiceCount == 0 || DiceSides == 0 || Multiplier == 0 || Coin.IsInvalid();

    public void Copy(CoinRoller roller)
    {
        VerifyCopy(this, roller);
        TreasureLevel = TreasureLevel == 0 ? TreasureLevel : roller.TreasureLevel;
        RollMin = RollMin == 0 ? RollMin : roller.RollMin;
        DiceCount = DiceCount == 0 ? DiceCount : roller.DiceCount;
        DiceSides = DiceSides == 0 ? DiceSides : roller.DiceSides;
        Multiplier = Multiplier == 0 ? Multiplier : roller.Multiplier;
    }
}