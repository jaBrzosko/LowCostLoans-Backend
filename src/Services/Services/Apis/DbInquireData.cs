namespace Services.Services.Apis;

public class DbInquireData
{
    public Guid Id { get; set; }
    public PersonalData PersonalData { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
}

public record PersonalData(string FirstName, string LastName, string GovernmentId, GovernmentIdType GovernmentIdType, JobType JobType);

public enum GovernmentIdType
{
    Pesel = 0,
}

public enum JobType
{
    SomeJobType = 0,
}
