using Data.Entities;

namespace Manager.Models;

public class RollerResult
{
    public RollerResult()
    {
        Cash = new Coin();
    }

    public Coin Cash { get; set; }
    public int Gems { get; set; }
    public int Items { get; set; }
}