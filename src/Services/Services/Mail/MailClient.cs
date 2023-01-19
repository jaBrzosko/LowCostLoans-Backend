using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace Services.Services.Mail;

public class MailClient
{
    private MailConfiguration cfg;
    private int port;
    private bool useSsl;

    public MailClient(MailConfiguration config)
    {
        cfg = config;
        port = int.Parse(cfg.Port);
        useSsl = bool.Parse(cfg.UseSsl);
    }

    public virtual async Task SendMail(string Recipient, string RecipientMail, string Subject, string Body, string BodyType, CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(cfg.Sender, cfg.SenderMail));
        message.To.Add(new MailboxAddress(Recipient, RecipientMail));
        message.Subject = Subject;

        message.Body = new TextPart(BodyType) { Text = Body };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(cfg.Host, port, useSsl, ct);
            await client.AuthenticateAsync(cfg.User, cfg.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}