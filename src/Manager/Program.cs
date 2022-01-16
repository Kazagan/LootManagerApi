using Manager.Models;

namespace Manager;

public static class Program
{
    public static void Main(string[] args)
    {
        var start = new Startup();
        start.Read();
        start.Write();
    }
}