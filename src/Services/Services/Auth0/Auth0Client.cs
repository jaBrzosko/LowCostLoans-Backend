using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;

namespace Services.Services.Auth0;

public class Auth0Client
{
    private readonly HttpClient client;

    public Auth0Client(HttpClient client) =>
        this.client = client;
    
    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService<Auth0Configuration>()!;
        client.BaseAddress = new Uri(configuration.ApiUrl);
    }
    
    public virtual async Task<Auth0Profile?> GetProfile(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync((string?)null);
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Auth0Profile>(body);
    }
}