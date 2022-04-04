using Microsoft.EntityFrameworkCore.Internal;

namespace Data.Entities;

public class Coin : Entity
{
    public Coin()
    {
        Name = "";
    }

    public decimal InGold { get; set; }
    public string Name { get; set; }
}
