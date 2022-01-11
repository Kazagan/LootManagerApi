namespace Manager.Models;

public class Coin
{
    public Coin(double inGold)
    {
        InGold = inGold;
    }
    public int Count { get; set; }
    public double InGold { get; }
}

public static class Coins
{
    public static Coin Iron = new Coin(.001);
    public static Coin HalfCopper = new Coin(.005);
    public static Coin Copper = new Coin(.01);
    public static Coin Nickel = new Coin(.05);
    public static Coin Silver = new Coin(.1);
    public static Coin Electrum = new Coin(.5);
    public static Coin Gold = new Coin(1);
    public static Coin Platinum = new Coin(10);
}