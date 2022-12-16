using System.Net;
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
        Domain.Offers.Offer offer;
        try
        {
            offer = await offersRepository.FindAndEnsureExistence(req.OfferId, ct);
        }
        catch (ArgumentException)
        {
            var errorResponse = new { errorCode = PostAcceptOffer.ErrorCodes.OfferDoesNotExist, errorMessage = "Offer Does Not Exist" };
            await SendAsync(errorResponse, (int)HttpStatusCode.BadRequest, cancellation: ct);
            return;
        }
        
        if (offer.SourceBank == OfferSourceBank.OurBank)
        {
            // TODO: Handle it via event
            await ourApiClient.PostAcceptOffer(offer.BankId, req.Contract, ct);
        }
        await SendAsync(new object(), cancellation: ct);
    }
}