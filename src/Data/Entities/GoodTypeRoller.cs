namespace Data.Entities;

public class GoodTypeRoller : Entity
{
    public GoodTypeRoller()
    {
        GoodType = new GoodType();
    }
    public int TreasureLevel { get; set; }
    public int RollMin { get; set; }
    public GoodType GoodType { get; set; }
    public override bool IsInvalid()
    {
        throw new NotImplementedException();
    }
}