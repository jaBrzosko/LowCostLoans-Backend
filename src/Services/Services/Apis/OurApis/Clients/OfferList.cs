namespace Services.Services.Apis.OurApis.Clients;

internal class OfferList
{
    public Guid InquireId { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public List<Offer> Offers { get; set; }
}
