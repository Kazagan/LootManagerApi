using Data.Enums;

namespace Data.Models;

public class Good
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Coin Value { get; set; } 
    public GoodType Type { get; set; }
}