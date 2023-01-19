using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data;
using Contracts.Offers;
using Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Data.Repositories;
using Services.Services.Apis.OurApis.Clients;
using Services.Services.Mail;

namespace Services.Endpoints.Offers;

[HttpGet("/sendMail")]
[AllowAnonymous]

public class GetSendMail : EndpointWithoutRequest
{
    private readonly MailClient client;

    public GetSendMail(MailClient mailClient)
    {
        client = mailClient;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            await client.SendMail("Debug Recipient", "debug.mail.xmax@slmail.me", "Hello!", "Debug works", "plain", ct);
            await SendOkAsync(ct);
        }
        catch (Exception)
        {
            await SendNotFoundAsync(ct);
        }
    }
}