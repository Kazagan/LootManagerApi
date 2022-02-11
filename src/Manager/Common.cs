using System.Reflection.Metadata;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Manager;

public static class Common
{
    /// <summary>
    /// Verifies all properties in the object.
    /// </summary>
    /// <param name="verify"></param>
    /// <returns>Name of first null or default object or empty string</returns>
    public static string VerifyObject(object verify)
    {
        var x = verify.GetType().GetProperties();
        foreach (var property in x)
        {
            var propertyName = property.Name;
            var y = verify.GetType().GetProperty(propertyName)?.GetValue(verify, null);
            var z = y.GetType().GetDefaultValue();
            if (y is not null && y == z) continue;
            return propertyName;
        }
        return "";
    }
}