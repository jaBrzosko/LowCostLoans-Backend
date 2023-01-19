using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class InquireDetailedResponse
{
    [JsonPropertyName("inquireId")]
    public int InquireId { get; set; }
    [JsonPropertyName("createDate")]
    public DateTime CreateDate { get; set; }
    [JsonPropertyName("statusId")]
    public int StatusId { get; set; }
    [JsonPropertyName("statusDescription")]
    public string StatusDescription { get; set; }
    [JsonPropertyName("offerId")]
    public int? OfferId { get; set; }
}