using Contracts.Examples;
using FastEndpoints;

namespace Services.Endpoints.Examples;

public class PostExampleEndpoint : Endpoint<PostExample>
{
    public override void Configure()
    {
        Post("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PostExample req, CancellationToken ct)
    {
        await SendAsync(new object(), cancellation: ct);
    }
}
