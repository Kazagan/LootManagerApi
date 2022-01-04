namespace Manager.Models;

public class Item
{
    public Item(string name, int cost, int count)
    {
        Name = name;
        Cost = cost;
        Count = count;
    }
    public string? Name { get; set; }
    public int Cost { get; set; }
    public int Count { get; set; }
}