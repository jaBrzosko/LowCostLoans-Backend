using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data;
using Contracts.Offers;
using Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferStatus")]
[AllowAnonymous]

public class GetOfferStatusEndpoint : Endpoint<GetOfferStatus, OfferStatusDto?>
{
    private readonly CoreDbContext dbContext;
    private readonly OurApiClient ourApiClient;

    public GetOfferStatusEndpoint(CoreDbContext dbContext, OurApiClient ourApiClient)
    {
        this.dbContext = dbContext;
        this.ourApiClient = ourApiClient;
    }

    public override async Task HandleAsync(GetOfferStatus req, CancellationToken ct)
    {
        var offer = await dbContext
            .Offers
            .Where(o => o.Id == req.Id)
            .FirstOrDefaultAsync(ct);
        if (offer is null)
            return;
        OfferStatusDto? retOffer = null;
        switch (offer.SourceBank)
        {
            case(OfferSourceBank.OurBank):
            {
                var response = await ourApiClient.GetOfferStatus(offer.BankId, ct);
                retOffer = new OfferStatusDto
                {
                    Id = req.Id,
                    Status = response
                };
                break;
            }
        }

        await SendAsync(retOffer, cancellation: ct);
    }
}