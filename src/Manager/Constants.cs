using Data.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Manager;

public static class Constants
{
    public const string Success = "Success";

    public static bool IsDefault<TEntity>(TEntity entity) where TEntity : Entity
    {
        var isDefault = false;
        foreach (var property in typeof(TEntity).GetProperties())
        {
            property.GetValue(entity, null);
            if (TypeExtensions.IsDefaultValue(property.GetType(), property))
            {
                isDefault = true;
            }
        }

        return isDefault;
    }
}