using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IRepository
{
    IQueryable<TEntity> Get<TEntity>() where TEntity : Entity;
    TEntity? Get<TEntity>(Guid id) where TEntity : Entity;
    Task Insert<TEntity>(TEntity entity) where TEntity : Entity;
    Task Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;
    Task Delete<TEntity>(TEntity entity) where TEntity : Entity;
    Task Update<TEntity>(TEntity entity) where TEntity : Entity;
    Task Save();
}
public class Repository<TContext> : IRepository where TContext : DbContext
{
    private TContext Context { get; }

    public Repository(TContext context) => Context = context;

    public IQueryable<TEntity> Get<TEntity>() where TEntity : Entity
    {
        return Context.Set<TEntity>();
    }

    public TEntity? Get<TEntity>(Guid id) where TEntity : Entity
    {
        return Context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    }

    public async Task Insert<TEntity>(TEntity entity) where TEntity : Entity
    {
        await Task.Run(() => Context.Add(entity));
    }

    public async Task Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
    {
        await Task.Run(() => Context.Set<TEntity>().AddRange(entities));
    }

    public async Task Update<TEntity>(TEntity entity) where TEntity : Entity
    {
        await Task.Run(() => Context.Update(entity));
    }

    public async Task Delete<TEntity>(TEntity entity) where TEntity : Entity
    {
        await Task.Run(() => Context.Set<TEntity>().Remove(entity));
    }

    public async Task Save()
    {
        await Context.SaveChangesAsync();
    }
}