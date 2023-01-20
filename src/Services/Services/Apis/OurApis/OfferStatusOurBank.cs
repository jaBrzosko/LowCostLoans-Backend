using Contracts.Offers;

namespace Services.Services.Apis.OurApis;

public class OfferStatusOurBank
{
    public Guid Id { get; set; }
    public OfferStatusTypeDto Status { get; set; }
}