using Contracts.Common;
using Contracts.Inquires;
using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Inquires;

[HttpGet("/inquiries/getInquireByUser")]
public class GetInquireByUserEndpoint: Endpoint<GetInquireByUser, PaginationResultDto<InquireDetailsDto>>
{
    private readonly CoreDbContext dbContext;
    private const int minPageSize = 1;
    private const int maxPageSize = 100;
    
    public GetInquireByUserEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquireByUser req, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(req.UserId))
        {
            var resultEmpty = new PaginationResultDto<InquireDetailsDto>
            {
                Results = new List<InquireDetailsDto>(),
                Offset = 0,
                TotalCount = 0
            };
            await SendAsync(resultEmpty, cancellation: ct);
            return;
        }
        
        int start = req.PageNumber * Math.Clamp(req.PageSize, minPageSize, maxPageSize);
        var query = dbContext
            .Inquiries
            .Where(u => u.UserId == req.UserId);
        var inqs = await query
            .Skip(start)
            .Take(Math.Clamp(req.PageSize, minPageSize, maxPageSize))
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
                        CreationTime = o.CreationTime,
                    })
                    .ToList(),
            })
            .ToListAsync(ct);
        var count = await query
            .CountAsync(ct);
        var result = new PaginationResultDto<InquireDetailsDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };
        await SendAsync(result, cancellation: ct);
    }
}