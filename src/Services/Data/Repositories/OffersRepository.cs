using Domain.Offers;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public class OffersRepository : Repository<Offer, Guid>
{
    public OffersRepository(CoreDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<Offer?> FindAsync(Guid id, CancellationToken ct)
    {
        return DbContext.Offers.FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}
