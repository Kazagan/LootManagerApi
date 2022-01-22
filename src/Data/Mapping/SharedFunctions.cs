namespace Data.Mapping;

public static class SharedFunctions
{
    public static int GetEnumMaxLength<TEnum>() where TEnum : Enum
    {
        return Enum.
            GetNames(typeof(TEnum))
            .Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)
            .Length;
    }
}