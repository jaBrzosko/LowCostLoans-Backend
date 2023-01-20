using Domain.Offers;

namespace Domain.Inquires;

public class PendingInquire
{
    public string BankInquireId { get; set; }
    public Guid InquireId { get; set; }
    public OfferSourceBank SourceBank { get; set; }
}