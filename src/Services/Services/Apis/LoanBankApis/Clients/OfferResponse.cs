using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class OfferResponse
{
    [JsonPropertyName("id")]
    public int OfferId { get; set; }
    [JsonPropertyName("percentage")]
    public float Percentage { get; set; }
    [JsonPropertyName("monthlyInstallment")]
    public float MonthlyInstallment { get; set; }
    [JsonPropertyName("requestedValue")]
    public double RequestedValue { get; set; }
    [JsonPropertyName("requestedPeriodInMonth")]
    public int NumberOfInstallments { get; set; }
    [JsonPropertyName("statusId")]
    public OfferStatus StatusId { get; set; }
    [JsonPropertyName("statusDescription")]
    public string StatusDescription { get; set; }
    [JsonPropertyName("inquireId")]
    public int InquireId { get; set; }
    [JsonPropertyName("createDate")]
    public DateTime CreateDate { get; set; }
    [JsonPropertyName("updateDate")]
    public DateTime UpdateDate { get; set; }
    [JsonPropertyName("approvedBy")]
    public string? ApprovedBy { get; set; }
    [JsonPropertyName("documentLink")]
    public string? DocumentLink { get; set; }
    [JsonPropertyName("documentLinkValidDate")]
    public DateTime? DocumentLinkValidDate { get; set; }
    
    
}