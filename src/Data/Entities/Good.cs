namespace Data.Entities;

public class Good : Entity
{
    public string Name { get; set; } = "";
    public Coin Coin { get; set; } = new();
    public GoodType GoodType { get; set; } = new();
    public bool IsInvalid()
    {
        throw new NotImplementedException();
    }
}