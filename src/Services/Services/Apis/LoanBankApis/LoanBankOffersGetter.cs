using Microsoft.AspNetCore.Mvc.Filters;
using Services.Services.Apis.LoanBankApis.Clients;

namespace Services.Services.Apis.LoanBankApis;

public class LoanBankOffersGetter: IApiOffersGetter
{
    private readonly LoanBankClient loanBankClient;
    private readonly LoanBankAuthClient authClient;

    public LoanBankOffersGetter(LoanBankClient loanBankClient, LoanBankAuthClient loanBankAuthClient)
    {
        this.loanBankClient = loanBankClient;
        this.authClient = loanBankAuthClient;
    }
    public async Task<List<ApiOfferData>> GetOffersAsync(DbInquireData dbInquireData, CancellationToken ct)
    {
        var postedInquireId = await loanBankClient.PostInquireAsync(dbInquireData, authClient, ct);
        if (postedInquireId is null)
        {
            return new();
        }
        
        // Insert data to be processed later
        
        return new List<ApiOfferData>();
    }
    
}