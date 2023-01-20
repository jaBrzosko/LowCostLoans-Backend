using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class AuthClientRequest
{
    [JsonPropertyName("grant_type")] 
    public string GrantType { get; } = "client_credentials";
    [JsonPropertyName("scope")] 
    public string Scope { get; } = "MiNI.LoanBank.API";
}