using FastEndpoints;

namespace Contracts.Users;

public class GetMyPersonalData
{
    [FromClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
    public string UserId { get; set; }
}
