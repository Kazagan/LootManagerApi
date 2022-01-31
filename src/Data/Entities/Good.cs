using Data.Enums;

namespace Data.Entities;

public class Good
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Coin Value { get; set; } 
    public GoodType Type { get; set; }
}