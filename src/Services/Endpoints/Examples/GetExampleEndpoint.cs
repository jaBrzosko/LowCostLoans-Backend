using Contracts.Examples;
using FastEndpoints;

namespace Services.Endpoints.Examples;

public class GetExampleEndpoint : Endpoint<GetExample, ExampleDto>
{
    public override void Configure()
    {
        Get("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetExample req, CancellationToken ct)
    {
        var result = new ExampleDto
        {
            Id = 1,
            Name = "asdf",
            CreationTime = DateTime.Now,
        };
        await SendAsync(result, cancellation: ct);
    }
}
