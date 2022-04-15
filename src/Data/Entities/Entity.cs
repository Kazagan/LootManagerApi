using Microsoft.EntityFrameworkCore.Internal;

namespace Data.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected void VerifyCopy(Entity original, Entity entity)
    {
        if (original is null || entity is null)
            throw new NullReferenceException();
        if (original.GetType() != entity.GetType())
            throw new ArgumentException();
    }
}