namespace Data.Entities;

public class Good : Entity
{
    public Good()
    {
        Name = "";
        Coin = new Coin();
    }
    public string Name { get; set; }
    public Coin Coin { get; set; }
    public GoodType GoodType { get; set; }
}