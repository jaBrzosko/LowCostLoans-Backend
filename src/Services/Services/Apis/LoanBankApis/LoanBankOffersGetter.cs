using Domain.Inquires;
using Domain.Offers;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Data;
using Services.Services.Apis.LoanBankApis.Clients;

namespace Services.Services.Apis.LoanBankApis;

public class LoanBankOffersGetter: IApiOffersGetter
{
    private readonly LoanBankClient loanBankClient;
    private readonly LoanBankAuthClient authClient;
    private readonly CoreDbContext coreDbContext;

    public LoanBankOffersGetter(LoanBankClient loanBankClient, LoanBankAuthClient loanBankAuthClient, CoreDbContext coreDbContext)
    {
        this.loanBankClient = loanBankClient;
        this.authClient = loanBankAuthClient;
        this.coreDbContext = coreDbContext;
    }
    public async Task<List<ApiOfferData>> GetOffersAsync(DbInquireData dbInquireData, CancellationToken ct)
    {
        var postedInquireId = await loanBankClient.PostInquireAsync(dbInquireData, authClient, ct);
        if (postedInquireId is not null)
        {
            var pendingInquire = new PendingInquire
            {
                BankInquireId = postedInquireId,
                SourceBank = OfferSourceBank.LoanBank,
                InquireId = dbInquireData.Id
            };

            await coreDbContext.PendingInquires.AddAsync(pendingInquire, ct);
            await coreDbContext.SaveChangesAsync(ct);
        }
        
        return new();
    }
    
}