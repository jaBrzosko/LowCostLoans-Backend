using Contracts.Examples;
using Domain.Examples;
using FastEndpoints;
using Microsoft.AspNetCore.Server.HttpSys;
using Services.Data;

namespace Services.Endpoints.Examples;

public class PostExampleEndpoint : Endpoint<PostExample>
{
    private readonly CoreDbContext dbContext;
    
    public PostExampleEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PostExample req, CancellationToken ct)
    {
        var example = new Example(req.Name);
        dbContext.Examples.Add(example);
        await dbContext.SaveChangesAsync(ct);
        await SendAsync(new object(), cancellation: ct);
    }
}
