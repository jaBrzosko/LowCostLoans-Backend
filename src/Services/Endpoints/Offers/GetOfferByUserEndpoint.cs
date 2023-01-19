using Contracts.Common;
using Contracts.Inquires;
using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Data;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferByUser")]
public class GetOfferByUserEndpoint : Endpoint<GetOfferByUser, PaginationResultDto<OfferDto>>
{
    private readonly CoreDbContext dbContext;
    private const int minPageSize = 1;
    private const int maxPageSize = 100;

    public GetOfferByUserEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetOfferByUser req, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(req.UserId))
        {
            var resultEmpty = new PaginationResultDto<OfferDto>
            {
                Results = new List<OfferDto>(),
                Offset = 0,
                TotalCount = 0
            };
            await SendAsync(resultEmpty, cancellation: ct);
            return;
        }

        int start = req.PageNumber * Math.Clamp(req.PageSize, minPageSize, maxPageSize);

        // get inquiries
        var userInquiries = await dbContext
            .Inquiries
            .Where(inq => inq.UserId == req.UserId)
            .Select(inq => inq.Id)
            .ToListAsync();
        var query = dbContext
            .Offers
            .Where(o => userInquiries.Contains(o.InquireId));
        var offers = await query
            .Skip(start)
            .Take(Math.Clamp(req.PageSize, minPageSize, maxPageSize))
            .Select(o => new OfferDto
            {
                Id = o.Id,
                InquireId = o.InquireId,
                MoneyInSmallestUnit = o.MoneyInSmallestUnit,
                NumberOfInstallments = o.NumberOfInstallments,
                InterestRateInPromiles = o.InterestRateInPromiles,
                CreationTime = o.CreationTime,
                SourceBank = OfferSourceBankDto.OurBank
            })
            .ToListAsync(ct);
        var count = await query
            .CountAsync(ct);
        var result = new PaginationResultDto<OfferDto>
        {
            Results = offers,
            Offset = start,
            TotalCount = count
        }; // TODO: use extension method like in API
        await SendAsync(result, cancellation: ct);
    }
}