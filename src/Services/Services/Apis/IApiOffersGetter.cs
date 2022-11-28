namespace Services.Services.Apis;

public interface IApiOffersGetter
{
    public Task<List<ApiOfferData>> GetOffersAsync(DbInquireData dbInquireData, CancellationToken ct);
}
