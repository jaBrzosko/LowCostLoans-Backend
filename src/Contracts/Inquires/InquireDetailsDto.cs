using Contracts.Offers;

namespace Contracts.Inquires;

public class InquireDetailsDto
{
    public Guid Id { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
    public InquireStatusDto Status { get; set; }
    public List<OfferDto> Offers { get; set; }
}
