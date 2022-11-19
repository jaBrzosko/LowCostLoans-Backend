namespace Contracts.Offers;

public class OfferDto
{
    public Guid Id { get; set; }
    public Guid InquireId { get; set; }
    public int InterestRateInPromiles { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
}
