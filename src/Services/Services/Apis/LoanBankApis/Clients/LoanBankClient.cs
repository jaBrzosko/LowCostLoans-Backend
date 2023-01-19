using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Offers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class LoanBankClient
{
    private readonly HttpClient client;

    public LoanBankClient(HttpClient client)
    {
        this.client = client;
    }
    
    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService<LoanBankConfiguration>()!;
        client.BaseAddress = new Uri(configuration.UrlPrefix);
    }

    public virtual async Task<ApiOfferData?> GetOfferAsync(string inquireId, LoanBankAuthClient authClient, CancellationToken ct)
    {
        var token = await authClient.GetTokenAsync(ct);
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var response = await client.GetAsync($"Inquire/{inquireId}", ct);
        var inquireDetails = await response.Content.ReadFromJsonAsync<InquireDetailedResponse>(cancellationToken: ct);
        if (inquireDetails is null || inquireDetails.OfferId is null)
        {
            return null;
        }
        var offerResponse = await client.GetAsync($"Offer/{inquireDetails.OfferId}", ct);
        var offerDetails = await offerResponse.Content.ReadFromJsonAsync<OfferResponse>(cancellationToken: ct);
        if (offerDetails is null)
        {
            return null;
        }
        var offer = new ApiOfferData
        {
            InterestRateInPromiles = (int)(offerDetails.Percentage * 100),
            MoneyInSmallestUnit = offerDetails.RequestedValue,
            NumberOfInstallments = offerDetails.NumberOfInstallments,
            BankId = offerDetails.OfferId.ToString()
        };
        return offer;
    }

    public virtual async Task<String?> PostInquireAsync(DbInquireData inquireData, LoanBankAuthClient authClient, CancellationToken ct)
    {
        var postInquire = new InquireRequest
        {
            Value = inquireData.MoneyInSmallestUnit,
            NumberOfInstallments = inquireData.NumberOfInstallments,
            PersonalData = new PersonalData
            {
                FirstName = inquireData.DbPersonalData.FirstName,
                LastName = inquireData.DbPersonalData.LastName,
                BirthDate = inquireData.DbPersonalData.GovernmentIdType == DbGovernmentIdType.Pesel ? 
                    PersonalData.ParsePesel(inquireData.DbPersonalData.GovernmentId) : new DateTime(1970, 1, 1)
            },
            GovernmentDocument = inquireData.DbPersonalData.GovernmentIdType == DbGovernmentIdType.Pesel ?
                new GovernmentDocument
                {
                    TypeId = 3,
                    Name = "Government Id",
                    Description = "Government Id",
                    Number = inquireData.DbPersonalData.GovernmentId
                } : new GovernmentDocument(),
            JobDetails = new JobDetails
            {
                TypeId = 30
            }
        };
        
        var token = await authClient.GetTokenAsync(ct);
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var content = JsonContent.Create(postInquire);
        var response = await client.PostAsync("Inquire", content, ct);
        var inquire = await response.Content.ReadFromJsonAsync<InquireResponse>(cancellationToken: ct);
        return inquire!.Id.ToString();
    }

    public virtual async Task<Uri> GetOfferContract(CancellationToken ct)
    {
        return new Uri("google.com");
    }

    public virtual async Task PostAcceptOffer(string offerId, IFormFile file, CancellationToken ct)
    {
    }

    public virtual async Task<OfferStatusTypeDto> GetOfferStatus(string offerId, CancellationToken ct)
    {
        return OfferStatusTypeDto.Created;
    }
}