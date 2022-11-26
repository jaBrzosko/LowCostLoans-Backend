using Contracts.Common;
using Contracts.Inquires;
using Domain.Inquires;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.DataMappers;
using Services.Data.Repositories;
using Services.Events.Inquires;

namespace Services.Endpoints.Inquires;

[HttpPost("/inquiries/createInquireAsAnonymous")]
[AllowAnonymous]
public class PostCreateInquireAsAnonymousEndpoint : Endpoint<PostCreateInquireAsAnonymous, PostResponseWithIdDto>
{
    private readonly InquiresRepository inquiriesRepository;

    public PostCreateInquireAsAnonymousEndpoint(InquiresRepository inquiriesRepository)
    {
        this.inquiriesRepository = inquiriesRepository;
    }

    public override async Task HandleAsync(PostCreateInquireAsAnonymous req, CancellationToken ct)
    {
        var inquire = new Inquire(null, req.PersonalData.ToNullableEntity(), req.MoneyInSmallestUnit, req.NumberOfInstallments);
        await inquiriesRepository.AddAsync(inquire, ct);
        await PublishAsync(new InquireCreatedEvent { InquireId = inquire.Id }, cancellation: ct);
        var response = new PostResponseWithIdDto { Id = inquire.Id };
        await SendAsync(response, cancellation: ct);
    }
}
