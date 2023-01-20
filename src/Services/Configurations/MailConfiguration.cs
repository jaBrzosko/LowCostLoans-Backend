namespace Services.Configurations;

public record MailConfiguration(string Host, string Port, string UseSsl, string User, string Password, string Sender, string SenderMail);
