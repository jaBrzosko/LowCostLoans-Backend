using System.Net.Http.Json;

namespace Services.Services.Apis.OurApis.Clients;

public class OurApiClient
{
    private readonly HttpClient client;

    public OurApiClient(HttpClient client)
    {
        this.client = client;
    }

    public static void Configure(HttpClient client)
    {
        client.BaseAddress = new Uri("http://localhost:8082"); // TODO: load it from configuration
    }

    public async Task<List<ApiOfferData>> GetOffersAsync(Guid inquireId, CancellationToken ct)
    {
        var response = await client.GetAsync($"offers/getOffersByInquireId?Id={inquireId}", ct);
        var offerList = await response.Content.ReadFromJsonAsync<OfferList>(cancellationToken: ct);
        return offerList?
            .Offers
            .Select(o => new ApiOfferData
            {
                InterestRateInPromiles = o.InterestRate,
                MoneyInSmallestUnit = offerList.MoneyInSmallestUnit,
                NumberOfInstallments = offerList.NumberOfInstallments,
            })
            .ToList()
            ??
            new();
    }

    public async Task<Guid?> PostInquireAsync(DbInquireData inquireData, CancellationToken ct)
    {
        var postInquire = new InquireRequest
        {
            MoneyInSmallestUnit = inquireData.MoneyInSmallestUnit,
            NumberOfInstallments = inquireData.NumberOfInstallments,
            PersonalData = new()
            {
                FirstName = inquireData.DbPersonalData.FirstName,
                LastName = inquireData.DbPersonalData.LastName,
                GovernmentId = inquireData.DbPersonalData.GovernmentId,
                GovernmentIdType = (GovernmentIdTypeDto)inquireData.DbPersonalData.GovernmentIdType,
                JobType = (JobTypeDto)inquireData.DbPersonalData.JobType,
            },
        };
        var content = JsonContent.Create(postInquire);
        var response = await client.PostAsync("inquiries/createInquireAsAnonymous", content, ct);
        var inquire = await response.Content.ReadFromJsonAsync<InquireResponse>(cancellationToken: ct);
        return inquire!.Id;
    }
}
