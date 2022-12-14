using Contracts.Inquires;
using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Inquires;

[HttpGet("/inquiries/getInquireDetailsById")]
[AllowAnonymous]
public class GetInquireDetailsByIdEndpoint : Endpoint<GetInquireDetailsById, InquireDetailsDto?>
{
    private readonly CoreDbContext dbContext;

    public GetInquireDetailsByIdEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquireDetailsById req, CancellationToken ct)
    {
        var result = await dbContext
            .Inquiries
            .Where(iq => iq.Id == req.Id)
            .Select(iq => new InquireDetailsDto
            {
                Id = iq.Id,
                MoneyInSmallestUnit = iq.MoneyInSmallestUnit,
                NumberOfInstallments = iq.NumberOfInstallments,
                CreationTime = iq.CreationTime,
                Status = (InquireStatusDto)iq.Status,
                Offers = dbContext
                    .Offers
                    .Where(o => o.InquireId == iq.Id)
                    .Select(o => new OfferDto
                    {
                        Id = o.Id,
                        InquireId = o.InquireId,
                        MoneyInSmallestUnit = o.MoneyInSmallestUnit,
                        NumberOfInstallments = o.NumberOfInstallments,
                        InterestRateInPromiles = o.InterestRateInPromiles,
                        SourceBank = (OfferSourceBankDto)o.SourceBank,
                        CreationTime = o.CreationTime,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(ct);
        
        await SendAsync(result, cancellation: ct);
    }
}
