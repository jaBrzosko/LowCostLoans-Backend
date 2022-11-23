using Contracts.Examples;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Services.Endpoints.AuthTest;

[HttpGet("/auth/no")]
[AllowAnonymous]
public class GetUnauthorizedEndpoint : Endpoint<GetAllExamples, string>
{
    public GetUnauthorizedEndpoint()
    { }

    public override async Task HandleAsync(GetAllExamples req, CancellationToken ct)
    {   
        await SendAsync("Hello, Guest!", cancellation: ct);
    }
}
