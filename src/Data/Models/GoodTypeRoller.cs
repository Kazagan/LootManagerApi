using Data.Enums;

namespace Data.Models;

public class GoodTypeRoller
{
    public int Id { get; set; }
    public int TreasureLevel { get; set; }
    public int RollMin { get; set; }
    public int RollMax { get; set; }
    public GoodType Type { get; set; }
}