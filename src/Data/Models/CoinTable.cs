namespace Data.Models;

public class CoinTable
{
    public int Id { get; set; }
    public int TreasureLevel { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public Coin Coin { get; set; }
    public int DiceCount { get; set; }
    public int DiceSides { get; set; }
    public int Multiplier { get; set; }

    public bool InRange(int x) => x <= Max && x >= Min;
}

// public static class CoinTableRows // TODO Seed in Database, and move these to test, and mock repository response.
// {
//     public static List<CoinTable> GetCoinTable()
//     {
//         return new List<CoinTable>
//         {
//             new(1, 1, 14, Coins.Copper, 2, 6, 1000),
//             new(1, 15, 29, Coins.Nickel, 4, 6, 100),
//             new(1, 30, 52, Coins.Silver, 2, 6, 100),
//             new(1, 53, 95, Coins.Gold, 2, 8, 10),
//             new(1, 96, 100, Coins.Platinum, 1, 4, 10)
//         };
//     }
// }