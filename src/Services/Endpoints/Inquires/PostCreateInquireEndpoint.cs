using Contracts.Common;
using Contracts.Inquires;
using Domain.Inquires;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.DataMappers;
using Services.Data.Repositories;
using Services.Events.Inquires;

namespace Services.Endpoints.Inquires;

[HttpPost("/inquiries/createInquire")]
[AllowAnonymous]
public class PostCreateInquireEndpoint : Endpoint<PostCreateInquire, PostResponseWithIdDto>
{
    private readonly InquiresRepository inquiriesRepository;

    public PostCreateInquireEndpoint(InquiresRepository inquiriesRepository)
    {
        this.inquiriesRepository = inquiriesRepository;
    }

    public override async Task HandleAsync(PostCreateInquire req, CancellationToken ct)
    {
        var inquire = new Inquire(req.UserId, null, req.MoneyInSmallestUnit, req.NumberOfInstallments);
        await inquiriesRepository.AddAsync(inquire, ct);
        await PublishAsync(new InquireCreatedEvent { InquireId = inquire.Id }, Mode.WaitForNone, ct);
        var response = new PostResponseWithIdDto { Id = inquire.Id };
        await SendAsync(response, cancellation: ct);
    }
}