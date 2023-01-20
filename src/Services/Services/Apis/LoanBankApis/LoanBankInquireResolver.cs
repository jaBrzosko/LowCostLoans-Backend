using Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Services.Apis.LoanBankApis.Clients;

namespace Services.Services.Apis.LoanBankApis;

public class LoanBankInquireResolver
{
    private readonly LoanBankClient loanBankClient;
    private readonly LoanBankAuthClient authClient;
    private readonly CoreDbContext coreDbContext;
    public LoanBankInquireResolver(LoanBankClient loanBankClient, LoanBankAuthClient authClient, CoreDbContext coreDbContext)
    {
        this.loanBankClient = loanBankClient;
        this.authClient = authClient;
        this.coreDbContext = coreDbContext;
    }

    public async Task ResolvePendingInquiries(CancellationToken ct)
    {
        var pendingInquires = await coreDbContext
            .PendingInquiries
            .Where(x => x.SourceBank == OfferSourceBank.LoanBank)
            .ToListAsync(ct);
        bool areChanges = false;
        foreach (var inq in pendingInquires)
        {
            var apiOffer = await loanBankClient.GetOfferAsync(inq.BankInquireId, authClient, ct);
            if (apiOffer is null)
            {
                continue;
            }
            var offer = new Offer(inq.InquireId, apiOffer.InterestRateInPromiles, apiOffer.MoneyInSmallestUnit, apiOffer.NumberOfInstallments, OfferSourceBank.LoanBank, apiOffer.BankId);
            coreDbContext.Offers.Add(offer);
            coreDbContext.PendingInquiries.Remove(inq);
            areChanges = true;
        }

        if (areChanges)
        {
            await coreDbContext.SaveChangesAsync(ct);
        }
    }
}