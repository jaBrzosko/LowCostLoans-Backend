using Domain.Users;

namespace Domain.Inquires;

public class Inquire
{
    public Guid Id { get; private init; }
    public string? UserId { get; private init; }
    public PersonalData? PersonalData { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }
    public InquireStatus Status { get; private set;  }

    public Inquire(string? userId, PersonalData? personalData, int moneyInSmallestUnit, int numberOfInstallments)
    {
        Validate(userId, personalData, moneyInSmallestUnit, numberOfInstallments);
        
        Id = Guid.NewGuid();
        UserId = userId;
        PersonalData = personalData;
        MoneyInSmallestUnit = moneyInSmallestUnit;
        NumberOfInstallments = numberOfInstallments;
        CreationTime = DateTime.UtcNow;
        Status = InquireStatus.Unprocessed;
    }
    
    private Inquire() 
    { }

    private static void Validate(string? userId, PersonalData? personalData, int moneyInSmallestUnit, int numberOfInstallments)
    {
        if (!(userId is null ^ personalData is null))
        {
            throw new ArgumentException("Exactly one of userId and personalData has to be null");
        }

        if (moneyInSmallestUnit <= 0)
        {
            throw new ArgumentException("Money has to be positive");
        }

        if (numberOfInstallments <= 0)
        {
            throw new ArgumentException("NumberOfInstallments has to be positive");
        }
    }

    public void UpdateStatus(InquireStatus status)
    {
        Status = status;
    }
}
