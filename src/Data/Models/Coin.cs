using Data.Enums;

namespace Data.Models;

public class Coin
{
    public int Id { get; set; }
    public CoinType CoinType { get; set; }
    public double InGold { get; set; }
    public int Count { get; set; }
}
