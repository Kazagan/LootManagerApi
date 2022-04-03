using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data;

public static class ColumnTypes
{
    private const string VarcharType = "Varchar";
    public static PropertyBuilder VarcharWithMaxLength(this PropertyBuilder builder, int maxLength)
    {
        builder
            .HasColumnType(VarcharType)
            .HasMaxLength(maxLength);
        return builder;
    }
}