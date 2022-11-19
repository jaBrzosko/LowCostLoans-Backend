using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public class Repository<TEntity> where TEntity : class, IDbEntity
{
    private readonly CoreDbContext dbContext;

    public Repository(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Add(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task RemoveAsync(TEntity entity, CancellationToken ct)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<TEntity> FindAndEnsureExistence(Guid id, CancellationToken ct)
    {
        var result = await dbContext
            .Set<TEntity>()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);

        if (result is null)
        {
            throw new ArgumentException("Entity with this Id does not exist");
        }

        return result;
    }
    
    public void Add(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }
    
    public void Remove(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }
}
