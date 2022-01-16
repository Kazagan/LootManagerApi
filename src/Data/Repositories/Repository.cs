using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public interface IRepository<TContext> where TContext : DbContext
{
    IQueryable<TEntity> Get<TEntity>() where TEntity : class;
    void Insert<TEntity>(TEntity entity) where TEntity : class;
    void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    void Delete<TEntity>(TEntity entity) where TEntity : class;
    void Save();
}
public class Repository<TContext> : IRepository<TContext> where TContext : DbContext
{
    private TContext Context { get; }

    public Repository(TContext context) => Context = context;

    public IQueryable<TEntity> Get<TEntity>() where TEntity : class
    {
        return Context.Set<TEntity>();
    }

    public void Insert<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void Save()
    {
        Context.SaveChanges();
    }
}