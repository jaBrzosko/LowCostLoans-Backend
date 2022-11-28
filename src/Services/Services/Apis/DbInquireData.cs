namespace Services.Services.Apis;

public class DbInquireData
{
    public Guid Id { get; set; }
    public DbPersonalData DbPersonalData { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
}

public record DbPersonalData(string FirstName, string LastName, string GovernmentId, DbGovernmentIdType GovernmentIdType, DbJobType JobType);

public enum DbGovernmentIdType
{
    Pesel = 0,
}

public enum DbJobType
{
    SomeJobType = 0,
}
