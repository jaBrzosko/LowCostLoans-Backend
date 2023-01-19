using Contracts.Common;
using FastEndpoints;

namespace Contracts.Offers;

public class GetOfferByUser : GetPaginatedList
{
    [FromClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
    public string UserId { get; set; }
}