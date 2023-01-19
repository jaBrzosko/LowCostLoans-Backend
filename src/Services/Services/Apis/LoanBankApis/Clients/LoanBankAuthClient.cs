using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contracts.Offers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class LoanBankAuthClient
{
    private readonly FormUrlEncodedContent content;
    private readonly HttpClient client;

    public LoanBankAuthClient(HttpClient client)
    {
        this.client = client;
        var pairs = new List<KeyValuePair<string, string>>();
        pairs.Add(new("grant_type", "client_credentials"));
        pairs.Add(new("scope", "MiNI.LoanBank.API"));   
        content = new FormUrlEncodedContent(pairs);
    }
    
    public static void Configure(IServiceProvider serviceProvider, HttpClient client)
    {
        var configuration = serviceProvider.GetService<LoanBankConfiguration>()!;
        client.BaseAddress = new Uri(configuration.AuthUrlPrefix);
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + 
            Base64Encode(configuration.ApiKeyName + ":" + configuration.ApiKeySecret));
    }
 
    public async Task<string> GetTokenAsync(CancellationToken ct)
    {
        var response = await client.PostAsync("connect/token", content, ct);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthClientResponse>(cancellationToken: ct);
        if(authResponse is null)
            return String.Empty;
        return authResponse.AccessToken;
    }
    
    private static string Base64Encode(string plainText) {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}