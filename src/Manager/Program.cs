using Manager.Models;

namespace Manager;

public static class Program
{
    public static void Main(string[] args)
    {
        var start = new Startup();

        var item = new Item("Sword", 1, 1);
        start.Write(item);
        start.Read();
    }
}