using Services.Services.Apis.LoanBankApis.Clients;

namespace Services.Services.Apis.LoanBankApis;

public class LoanBankOffersGetter: IApiOffersGetter
{
    private readonly LoanBankClient loanBankClient;

    public LoanBankOffersGetter(LoanBankClient loanBankClient)
    {
        this.loanBankClient = loanBankClient;
    }
    public async Task<List<ApiOfferData>> GetOffersAsync(DbInquireData dbInquireData, CancellationToken ct)
    {
        var postedInquireId = await loanBankClient.PostInquireAsync(dbInquireData, ct);
        if (postedInquireId is null)
        {
            return new();
        }
        
        // Insert data to be processed later
        
        return new List<ApiOfferData>();
    }
    
}