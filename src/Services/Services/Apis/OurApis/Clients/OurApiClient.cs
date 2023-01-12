using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Offers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;

namespace Services.Services.Apis.OurApis.Clients;

public class OurApiClient
{
    private const string ApiKeyHeaderName = "ApiKey";

    private readonly HttpClient client;

    public OurApiClient(HttpClient client)
    {
        this.client = client;
    }

    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService<OurApiConfiguration>()!;
        client.BaseAddress = new Uri(configuration.UrlPrefix);
        client.DefaultRequestHeaders.Add(ApiKeyHeaderName, configuration.ApiKey);
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
        var response = await client.PostAsync("inquiries/createAnonymousInquire", content, ct);
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
}
