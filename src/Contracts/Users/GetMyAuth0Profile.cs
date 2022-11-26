using FastEndpoints;

namespace Contracts.Users;

public class GetMyAuth0Profile
{
    [FromHeader(HeaderName = "Authorization", IsRequired = false)]
    public string TokenWithBearerPrefix { get; set; }
}
