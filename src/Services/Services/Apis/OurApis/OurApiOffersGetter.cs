using Services.Services.Apis.OurApis.Clients;

namespace Services.Services.Apis.OurApis;

public class OurApiOffersGetter : IApiOffersGetter
{
    private readonly OurApiClient apiClient;

    public OurApiOffersGetter(OurApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public async Task<List<ApiOfferData>> GetOffersAsync(DbInquireData dbInquireData, CancellationToken ct)
    {
        var postedInquireId = await apiClient.PostInquireAsync(dbInquireData, ct);

        if (postedInquireId is null)
        {
            return new();
        }
        
        return await apiClient.GetOffersAsync((Guid)postedInquireId, ct);
    }
}
