namespace Manager.Services;

public static class SharedFunctions
{
    public static int GetValue(int diceCount, int diceSides, int multiplier)
    {
        var roll = 0;
        for (var i = 0; i < diceCount; i++)
            roll += Random.Shared.Next(1, diceSides);
        return roll * multiplier;
    }
}