using Domain.Inquires;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public class InquiresRepository : Repository<Inquire, Guid>
{
    public InquiresRepository(CoreDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<Inquire?> FindAsync(Guid id, CancellationToken ct)
    {
        return DbContext.Inquiries.FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}
