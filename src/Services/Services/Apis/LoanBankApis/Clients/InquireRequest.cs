using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class InquireRequest
{
    //[JsonPropertyName("value")]
    public int Value { get; set; }
    
    //[JsonPropertyName("sub")]
    public int NumberOfInstallments { get; set; }
    
    //[JsonPropertyName("personalData")]
    public PersonalData  PersonalData { get; set; }

    //[JsonPropertyName("governmentDocument")]
    public GovernmentDocument GovernmentDocument { get; set; }
    
    //[JsonPropertyName("governmentId")]
    public  JobDetails JobDetails { get; set; }
}
