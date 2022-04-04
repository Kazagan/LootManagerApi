using Microsoft.EntityFrameworkCore.Internal;

namespace Data.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
}