using Data.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Manager;

public static class Constants
{
    public const string Success = "Success";
    public const string NotFound = "Not Found";
    public const string Exists = "Object already Exists";
    public const string Invalid = "Invalid";

    public static IEnumerable<string> BadResults = new List<string>
    {
        NotFound, Exists, Invalid
    };

}