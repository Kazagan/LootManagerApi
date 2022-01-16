using Data.Enums;

namespace Data.Models;

public class Coin
{
    public CoinType CoinType { get; set; }
    public double InGold { get; set; }
    public int Count { get; set; }
}

// public static class Coins // TODO Seed Into Db? Foriegn Key to CoinTable?
// {
//     public static Coin Iron = new Coin(.001, CoinType.Iron);
//     public static Coin HalfCopper = new Coin(.005, CoinType.HalfCopper);
//     public static Coin Copper = new Coin(.01, CoinType.Copper);
//     public static Coin Nickel = new Coin(.05, CoinType.Nickel);
//     public static Coin Silver = new Coin(.1, CoinType.Silver);
//     public static Coin Electrum = new Coin(.5, CoinType.Electrum);
//     public static Coin Gold = new Coin(1, CoinType.Gold);
//     public static Coin Platinum = new Coin(10, CoinType.Platinum);
// }

