using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferEndpoint: Endpoint<PostAcceptOffer>
{
    private OurApiClient ourApiClient;
    public PostAcceptOfferEndpoint(OurApiClient ourApiClient)
    {
        this.ourApiClient = ourApiClient;
    }

    public override void Configure()
    {
        AllowFileUploads();
        AllowAnonymous();
        Post("/offers/accept");
    }

    public override async Task HandleAsync(PostAcceptOffer req, CancellationToken ct)
    {
        var dto = new AcceptOfferDto
        {
            OfferId = req.OfferId,
            Contract = req.Contract
        };
        await ourApiClient.PostAcceptOffer(dto, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}