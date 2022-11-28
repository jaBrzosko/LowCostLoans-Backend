namespace Services.Services.Apis.OurApis.Clients;

internal class InquireRequest
{
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public PersonalData PersonalData { get; set; }
}
