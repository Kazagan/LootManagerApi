using Microsoft.EntityFrameworkCore.Internal;

namespace Data.Entities;

public class Coin : Entity
{
    public decimal InGold { get; set; }
    public string Name { get; set; } = "";

    public bool IsInvalid() =>
        InGold == 0 || string.IsNullOrEmpty(Name);

    public void Copy(Coin coin)
    {
        VerifyCopy(this, coin);
        Name = string.IsNullOrEmpty(coin.Name) ? Name : coin.Name;
        InGold = coin.InGold == 0 ? InGold : coin.InGold;
    }
}
