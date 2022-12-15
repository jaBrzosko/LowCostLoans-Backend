using Contracts.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data;
using Services.Data.Repositories;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferEndpoint: Endpoint<PostAcceptOffer>
{
    private OurApiClient ourApiClient;
    private OffersRepository offersRepository;
    public PostAcceptOfferEndpoint(OurApiClient ourApiClient, OffersRepository offersRepository)
    {
        this.ourApiClient = ourApiClient;
        this.offersRepository = offersRepository;
    }

    public override void Configure()
    {
        AllowFileUploads();
        AllowAnonymous();
        Post("/offers/accept");
    }

    public override async Task HandleAsync(PostAcceptOffer req, CancellationToken ct)
    {
        var offer = await offersRepository.FindAndEnsureExistence(req.OfferId, ct);
        
        if (offer.SourceBank == OfferSourceBank.OurBank)
        {
            await ourApiClient.PostAcceptOffer(offer.BankId, req.Contract, ct);
        }
        await SendAsync(new object(), cancellation: ct);
    }
}