using Domain;
using Domain.Examples;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public abstract class Repository<TEntity, TId> where TEntity : class
{
    protected readonly CoreDbContext DbContext;

    public Repository(CoreDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken ct)
    {
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(ct);
    }
    
    public async Task RemoveAsync(TEntity entity, CancellationToken ct)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public abstract Task<TEntity?> FindAsync(TId id, CancellationToken ct);

    public async Task<TEntity> FindAndEnsureExistence(TId id, CancellationToken ct)
    {
        var result = await FindAsync(id, ct);

        if (result is null)
        {
            throw new ArgumentException("Entity with this Id does not exist");
        }

        return result;
    }
    
    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }
    
    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }
}
