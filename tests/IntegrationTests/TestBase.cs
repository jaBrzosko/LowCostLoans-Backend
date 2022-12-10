using Api;
using Xunit;

namespace IntegrationTests;

public class TestBase : IClassFixture<ApiWebFactory>
{
    private readonly ApiWebFactory apiWebFactory;
    protected readonly HttpClient Client;

    public TestBase(ApiWebFactory apiWebFactory)
    {
        this.apiWebFactory = apiWebFactory;
        Client = apiWebFactory.CreateClient();
    }
}
