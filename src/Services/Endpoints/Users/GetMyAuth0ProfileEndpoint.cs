using Contracts.Users;
using FastEndpoints;
using Services.Data.Auth0;

namespace Services.Endpoints.Users;

[HttpGet("/users/getMyAuth0Profile")]
public class GetMyAuth0ProfileEndpoint : Endpoint<GetMyAuth0Profile, Auth0ProfileDto?>
{
    private readonly Auth0Client authClient;

    public GetMyAuth0ProfileEndpoint(Auth0Client authClient)
    {
        this.authClient = authClient;
    }

    public override async Task HandleAsync(GetMyAuth0Profile req, CancellationToken ct)
    {
        var token = GetToken(req);
        var profile = await authClient.GetProfile(token);
        await SendAsync(ToDto(profile), cancellation: ct);
    }

    private static string GetToken(GetMyAuth0Profile req)
    {
        return req.TokenWithBearerPrefix[7..];
    }

    private static Auth0ProfileDto? ToDto(Auth0Profile? profile)
    {
        if (profile is null)
        {
            return null;
        }

        return new()
        {
            Id = profile.Sub,
            Nickname = profile.Nickname,
            Name = profile.Name,
            PictureUrl = new Uri(profile.Picture),
            UpdateTime = profile.UpdatedAt,
            Mail = profile.Mail,
            IsMailVerified = profile.MailVerified,
        };
    }
}
