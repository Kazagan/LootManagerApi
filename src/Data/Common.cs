using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;

namespace Data;

public static class Common
{
    public static bool ValidInsert(object input)
    {
        var verified = true;
        foreach (var prop in input.GetType().GetProperties())
        {
            if (prop.PropertyType.IsDefaultValue(prop.GetValue(input, null)) &&
                !prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
            {
                verified = false;
            }
        }

        return verified;
    }
}