using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class GovernmentDocument
{
    [JsonPropertyName("typeId")]
    public int? TypeId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("number")]
    public string? Number { get; set; }
}