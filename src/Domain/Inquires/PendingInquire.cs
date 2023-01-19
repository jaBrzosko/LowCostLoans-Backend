using Domain.Offers;

namespace Domain.Inquires;

public class PendingInquire
{
    public string InquireId { get; set; }

    public OfferSourceBank SourceBank { get; set; }
}