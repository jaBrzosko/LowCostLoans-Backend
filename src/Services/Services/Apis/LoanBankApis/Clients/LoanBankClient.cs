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
    private readonly LoanBankAuthClient authClient;

    public LoanBankClient(HttpClient client, LoanBankAuthClient authClient)
    {
        this.client = client;
        this.authClient = authClient;
    }
    
    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService<OurApiConfiguration>()!;
        client.BaseAddress = new Uri(configuration.UrlPrefix);
    }

    public virtual async Task<List<ApiOfferData>> GetOffersAsync(Guid inquireId, CancellationToken ct)
    {
        return new List<ApiOfferData>();
    }

    public virtual async Task<String?> PostInquireAsync(DbInquireData inquireData, CancellationToken ct)
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