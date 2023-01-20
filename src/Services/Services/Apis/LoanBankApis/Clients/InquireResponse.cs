using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class InquireResponse
{
    [JsonPropertyName("inquireId")]
    public int Id { get; set; }
    [JsonPropertyName("createDate")]
    public DateTime CreateDate { get; set; }
}