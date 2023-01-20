using Contracts.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.Repositories;
using Services.Services.Apis.LoanBankApis.Clients;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferContract")]
[AllowAnonymous]
public class GetOfferContractEndpoint : Endpoint<GetOfferContract, ContractDto>
{
    private readonly OffersRepository offersRepository;
    private readonly OurApiClient ourApiClient;
    private readonly LoanBankClient loanBankClient;
    private readonly LoanBankAuthClient authClient;
    public GetOfferContractEndpoint(OffersRepository offersRepository, OurApiClient ourApiClient, 
        LoanBankClient loanBankClient, LoanBankAuthClient authClient)
    {
        this.offersRepository = offersRepository;
        this.ourApiClient = ourApiClient;
        this.loanBankClient = loanBankClient;
        this.authClient = authClient;
    }

    public override async Task HandleAsync(GetOfferContract req, CancellationToken ct)
    {
        var offer = await offersRepository.FindAndEnsureExistence(req.OfferId, ct);

        var result = new ContractDto();

        switch (offer.SourceBank)
        {
            case OfferSourceBank.OurBank:
            {
                result.ContractUrl = await ourApiClient.GetOfferContract(ct);
                break;
            }
            case OfferSourceBank.LoanBank:
            {
                result.ContractUrl = await loanBankClient.GetOfferContract(offer.BankId, authClient, ct);
                break;
            }
        }

        await SendAsync(result, cancellation: ct);
    }
}
