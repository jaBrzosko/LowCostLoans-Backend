using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class AuthClientRequest
{
    [JsonPropertyName("grant_type")] public static string GrantType { get; } = "client_credentials";
    [JsonPropertyName("scope")] public static string Scope { get; } = "MiNI.LoanBank.API";
}