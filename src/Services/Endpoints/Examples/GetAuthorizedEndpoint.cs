using Endpoints.Auxilliary;
using FastEndpoints;
using Services.Data.Auth0;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Services.Endpoints.AuthTest;

[HttpGet("/auth/yes")]
public class GetAuthorizedEndpoint : Endpoint<GetAccessToken, string>
{
    private Auth0Client authClient; 

    public GetAuthorizedEndpoint(Auth0Client client)
    {
        this.authClient = client;
    }

    public override async Task HandleAsync(GetAccessToken req, CancellationToken ct)
    {   
        // call Auth0 with received token
        var token = req.Authorization.Substring(7); // Skip "Bearer "
        var profile = await authClient.GetProfile(token);
        if (profile == null)
        {
            await SendAsync("", 400);
            return;
        }
        await SendAsync($"Hello. Auth0 says your mail is {((Auth0Profile)profile).Mail}", cancellation: ct);
    }
}
