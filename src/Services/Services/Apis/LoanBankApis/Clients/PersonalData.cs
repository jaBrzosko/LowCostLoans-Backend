using System.Text.Json.Serialization;

namespace Services.Services.Apis.LoanBankApis.Clients;

public class PersonalData
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; set; }

    public static DateTime ParsePesel(string pesel)
    {
        var year = pesel.Substring(0, 2);
        var month = pesel.Substring(2, 2);
        var day = pesel.Substring(4, 2);
        try
        {
            int yearInt = 1900 + Int32.Parse(year);
            int monthInt = Int32.Parse(month);
            int dayInt = Int32.Parse(day);
            
            while (monthInt > 20)
            {
                monthInt -= 20;
                yearInt += 100;
            }

            return new DateTime(yearInt, monthInt, dayInt);
        }
        catch (Exception e)
        {
            return new DateTime(1970, 1, 1);
        }
    }
}