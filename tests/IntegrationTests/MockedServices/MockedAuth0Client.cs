using Services.Services.Auth0;

namespace IntegrationTests.MockedServices;

public class MockedAuth0Client : Auth0Client
{
    public MockedAuth0Client(HttpClient client) : base(client)
    { }

    public override Task<Auth0Profile?> GetProfile(string token)
    {
        return Task.FromResult(new Auth0Profile()
        {
            Mail = "email@email.com",
            MailVerified = true,
            Name = "name",
            Nickname = "Nickname",
            Picture = "picture",
            Sub = "sub",
            UpdatedAt = new DateTime(),
        });
    }
}
