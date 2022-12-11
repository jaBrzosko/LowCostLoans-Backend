using Services.Services.Apis;
using Services.Services.Apis.OurApis.Clients;

namespace IntegrationTests.MockedServices;

public class MockedOurApiClient : OurApiClient
{
    public MockedOurApiClient(HttpClient client) : base(client)
    { }

    public override Task<List<ApiOfferData>> GetOffersAsync(Guid inquireId, CancellationToken ct)
    {
        return Task.FromResult(new List<ApiOfferData>()
        {
            new() { InterestRateInPromiles = 10, MoneyInSmallestUnit = 10000, NumberOfInstallments = 12 },
            new() { InterestRateInPromiles = 10, MoneyInSmallestUnit = 1000, NumberOfInstallments = 12 },
            new() { InterestRateInPromiles = 10, MoneyInSmallestUnit = 20000, NumberOfInstallments = 12 },
        });
    }

    public override Task<Guid?> PostInquireAsync(DbInquireData inquireData, CancellationToken ct)
    {
        return Task.FromResult<Guid?>(new Guid());
    }
}