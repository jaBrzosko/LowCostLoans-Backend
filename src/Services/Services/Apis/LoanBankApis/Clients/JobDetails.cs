using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class JobDetails
{
    [JsonPropertyName("typeId")]
    public int? TypeId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("jobStartDate")]
    public DateTime? JobStartDate { get; set; }
    [JsonPropertyName("jobEndDate")]
    public DateTime? JobEndDate { get; set; }
}