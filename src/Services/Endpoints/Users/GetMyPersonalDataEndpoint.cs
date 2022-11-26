using Contracts.Users;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Data.DataMappers;

namespace Services.Endpoints.Users;

[HttpGet("/users/getMyPersonalData")]
public class GetMyPersonalDataEndpoint : Endpoint<GetMyPersonalData, PersonalDataDto?>
{
    private readonly CoreDbContext dbContext;

    public GetMyPersonalDataEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetMyPersonalData req, CancellationToken ct)
    {
        var user = await dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id == req.UserId, ct);

        await SendAsync(user?.PersonalData.ToNullableDto(), cancellation: ct);
    }
}
