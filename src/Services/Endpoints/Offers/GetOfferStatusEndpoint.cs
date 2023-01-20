using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data;
using Contracts.Offers;
using Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Data.Repositories;
using Services.Services.Apis.LoanBankApis.Clients;
using Services.Services.Apis.OurApis.Clients;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferStatus")]
[AllowAnonymous]

public class GetOfferStatusEndpoint : Endpoint<GetOfferStatus, OfferStatusDto?>
{
    private readonly OffersRepository offersRepository;
    private readonly OurApiClient ourApiClient;
    private readonly LoanBankClient loanBankClient;
    public GetOfferStatusEndpoint(OffersRepository offersRepository, OurApiClient ourApiClient, LoanBankClient loanBankClient)
    {
        this.offersRepository = offersRepository;
        this.ourApiClient = ourApiClient;
        this.loanBankClient = loanBankClient;
    }

    public override async Task HandleAsync(GetOfferStatus req, CancellationToken ct)
    {
        var offer = await offersRepository.FindAsync(req.Id, ct);
        if (offer is null)
            return;
        OfferStatusDto? retOffer = null;
        try
        {
            switch (offer.SourceBank)
            {
                case (OfferSourceBank.OurBank):
                    {
                        var response = await ourApiClient.GetOfferStatus(offer.BankId, ct);
                        retOffer = new OfferStatusDto
                        {
                            Id = req.Id,
                            Status = response
                        };
                        break;
                    }
                case (OfferSourceBank.LoanBank):
                {
                    var response = await loanBankClient.GetOfferStatus(offer.BankId, ct);
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
        catch (Exception e)
        {
            await SendNotFoundAsync(ct);
        }
    }
}