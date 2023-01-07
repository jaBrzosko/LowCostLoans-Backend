using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Offers;
using Microsoft.AspNetCore.Http;

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
        client.BaseAddress = new Uri("http://api:80"); // TODO: load it from configuration
    }

    public virtual async Task<List<ApiOfferData>> GetOffersAsync(Guid inquireId, CancellationToken ct)
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
                BankId = o.Id.ToString()
            })
            .ToList()
            ??
            new();
    }

    public virtual async Task<Guid?> PostInquireAsync(DbInquireData inquireData, CancellationToken ct)
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

    public virtual async Task<Uri> GetOfferContract(CancellationToken ct)
    {
        var response = await client.GetAsync("offers/getOfferContract", ct);
        var contract = await response.Content.ReadFromJsonAsync<Contract>(cancellationToken: ct);
        return contract!.ContractUrl;
    }

    public virtual async Task PostAcceptOffer(string offerId, IFormFile file, CancellationToken ct)
    {
        var content = new MultipartFormDataContent();

        var offerIdContent = new StringContent(offerId);
        content.Add(offerIdContent, "OfferId");

        var contractContent = new StreamContent(file.OpenReadStream());
        contractContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "Contract",
            FileName = file.FileName
        };
        content.Add(contractContent);

        await client.PostAsync("offers/accept", content, ct);
    }

    public virtual async Task<OfferStatusTypeDto> GetOfferStatus(string offerId, CancellationToken ct)
    {
        var response = await client.GetAsync($"/offers/getOfferStatus?Id={offerId}", ct);
        var status = await response.Content.ReadFromJsonAsync<OfferStatusOurBank>(cancellationToken: ct);
        return status!.Status;
    }
}
