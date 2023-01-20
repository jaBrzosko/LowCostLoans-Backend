using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class InquireRequest
{
    [JsonPropertyName("value")]
    public int Value { get; set; }
    
    [JsonPropertyName("installmentsNumber")]
    public int NumberOfInstallments { get; set; }
    
    [JsonPropertyName("personalData")]
    public PersonalData  PersonalData { get; set; }

    [JsonPropertyName("governmentDocument")]
    public GovernmentDocument GovernmentDocument { get; set; }
    
    [JsonPropertyName("jobDetails")]
    public  JobDetails JobDetails { get; set; }
}
