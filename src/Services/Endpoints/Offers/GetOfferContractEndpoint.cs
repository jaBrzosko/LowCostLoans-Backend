using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.Repositories;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferContract")]
[AllowAnonymous]
public class GetOfferContractEndpoint : Endpoint<GetOfferContract, ContractDto>
{
    private readonly OffersRepository offersRepository;
    private readonly OurApiClient ourApiClient;

    public GetOfferContractEndpoint(OffersRepository offersRepository, OurApiClient ourApiClient)
    {
        this.offersRepository = offersRepository;
        this.ourApiClient = ourApiClient;
    }

    public override async Task HandleAsync(GetOfferContract req, CancellationToken ct)
    {
        var offer = await offersRepository.FindAndEnsureExistence(req.OfferId, ct);

        var result = new ContractDto
        {
            ContractUrl = await ourApiClient.GetOfferContract(ct),
        };

        await SendAsync(result, cancellation: ct);
    }
}
