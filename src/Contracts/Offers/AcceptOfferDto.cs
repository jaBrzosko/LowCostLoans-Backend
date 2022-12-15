using Microsoft.AspNetCore.Http;

namespace Contracts.Offers;

public class AcceptOfferDto
{
    public Guid OfferId { get; set; }
    public IFormFile Contract { get; set; }
}