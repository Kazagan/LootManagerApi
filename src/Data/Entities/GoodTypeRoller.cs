namespace Data.Entities;

public class GoodTypeRoller : Entity
{
    public int TreasureLevel { get; set; }
    public int RollMin { get; set; }
    public GoodType GoodType { get; set; }
}