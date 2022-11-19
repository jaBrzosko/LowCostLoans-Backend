namespace Domain.Offers;

public class Offer
{
    public Guid Id { get; private init; }
    public Guid InquireId { get; private init; }
    public int InterestRate { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }
    
    public Offer(Guid inquireId, int interestRate, int moneyInSmallestUnit, int numberOfInstallments)
    {
        Id = Guid.NewGuid();
        InquireId = inquireId;
        InterestRate = interestRate;
        MoneyInSmallestUnit = moneyInSmallestUnit;
        NumberOfInstallments = numberOfInstallments;
        CreationTime = DateTime.UtcNow;
    }
}
