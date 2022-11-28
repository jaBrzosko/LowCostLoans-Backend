using Domain.Examples;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public class ExamplesRepository : Repository<Example, Guid>
{
    public ExamplesRepository(CoreDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<Example?> FindAsync(Guid id, CancellationToken ct)
    {
        return DbContext.Examples.FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}
