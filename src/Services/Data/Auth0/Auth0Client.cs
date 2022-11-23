using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;

namespace Services.Data.Auth0;

public class Auth0Client
{
    private static string _endpointUrl = "";
    private HttpClient client;

    public Auth0Client(HttpClient client) =>
        this.client = client;

    public static void SetEndpointUrl(string newUrl) =>
        _endpointUrl = newUrl;
    
    public async Task<Auth0Profile?> GetProfile(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync((string?)null);
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Auth0Profile>(body);
    }
}