using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IRepository<TContext> where TContext : DbContext
{
    IQueryable<TEntity> Get<TEntity>() where TEntity : Entity;
    TEntity Get<TEntity>(int id) where TEntity : Entity;
    void Insert<TEntity>(TEntity entity) where TEntity : Entity;
    void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity;
    void Delete<TEntity>(TEntity entity) where TEntity : Entity;
    void Update<TEntity>(TEntity entity) where TEntity : Entity;
    void Save();
}
public class Repository<TContext> : IRepository<TContext> where TContext : DbContext
{
    private TContext Context { get; }

    public Repository(TContext context) => Context = context;

    public IQueryable<TEntity> Get<TEntity>() where TEntity : Entity
    {
        return Context.Set<TEntity>();
    }

    public TEntity? Get<TEntity>(int id) where TEntity : Entity
    {
        return Context.Set<TEntity>().FirstOrDefault(x => x.Id == id);
    }

    public void Insert<TEntity>(TEntity entity) where TEntity : Entity
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : Entity
    {
        Context.Update(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : Entity
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void Save()
    {
        Context.SaveChanges();
    }
}