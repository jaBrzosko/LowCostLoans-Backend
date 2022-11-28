using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Services.Data.Repositories;

public class UsersRepository : Repository<User, string>
{
    public UsersRepository(CoreDbContext dbContext) : base(dbContext)
    {
    }

    public override Task<User?> FindAsync(string id, CancellationToken ct)
    {
        return DbContext.Users.FirstOrDefaultAsync(e => e.Id == id, ct);
    }
}
