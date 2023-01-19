using Domain.Inquires;
using Domain.Offers;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Services.Data;
using Services.Data.Repositories;
using Services.Services.Apis;
using Services.Services.Apis.LoanBankApis;
using Services.Services.Apis.LoanBankApis.Clients;
using Services.Services.Apis.OurApis;

namespace Services.Events.Inquires;

public class InquireCreatedHandler : IEventHandler<InquireCreatedEvent>
{
    private List<IApiOffersGetter> apiOffersGetters = null!;
    private readonly IServiceScopeFactory scopeFactory;

    public InquireCreatedHandler(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(InquireCreatedEvent eventModel, CancellationToken ct)
    {
        File.WriteAllText("dupan0", "Dupan0");
        using var scope = scopeFactory.CreateScope();
        File.WriteAllText("dupan1", "Dupan1");
        ConstructApiOffersGetters(scope);
        File.WriteAllText("dupan2", "Dupan2");
        var inquiriesRepository = scope.ServiceProvider.GetService<InquiresRepository>()!;
        File.WriteAllText("dupan3", "Dupan3");
        var offersRepository = scope.ServiceProvider.GetService<OffersRepository>()!;
        File.WriteAllText("dupan4", "Dupan4");
        var usersRepository = scope.ServiceProvider.GetService<UsersRepository>()!;
        File.WriteAllText("dupan5", "Dupan5");
        var dbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        File.WriteAllText("dupan6", "Dupan6");
        
        var inquire = await inquiriesRepository.FindAndEnsureExistence(eventModel.InquireId, ct);
        File.WriteAllText("dupan7", "Dupan7");

        try
        {
            File.WriteAllText("dupa0", "Dupa0");

            foreach (var offersGetter in apiOffersGetters)
            {
                var apiOffers = await offersGetter.GetOffersAsync(await GetDbInquireDataAsync(inquire, usersRepository, ct), ct);
                foreach (var o in apiOffers)
                {
                    var offer = new Offer(inquire.Id, o.InterestRateInPromiles, o.MoneyInSmallestUnit, o.NumberOfInstallments, OfferSourceBank.OurBank, o.BankId);
                    offersRepository.Add(offer);
                }
            }

            inquire.UpdateStatus(InquireStatus.OffersGenerated);
            inquiriesRepository.Update(inquire);
        }
        catch (Exception exception)
        {
            inquire.UpdateStatus(InquireStatus.OffersGenerationFailed);
            inquiriesRepository.Update(inquire);
            File.WriteAllText("dupaYYY", exception.ToString());

        }

        await dbContext.SaveChangesAsync(ct);
    }

    private void ConstructApiOffersGetters(IServiceScope scope)
    {
        try
        {
            apiOffersGetters = new()
            {
                scope.ServiceProvider.GetService<OurApiOffersGetter>()!,
                scope.ServiceProvider.GetService<LoanBankOffersGetter>()!
            };
        }
        catch (Exception exception)
        {
            File.WriteAllText("dupaXX", exception.ToString());
        }
        
    }

    private async Task<DbInquireData> GetDbInquireDataAsync(Inquire inquire, UsersRepository usersRepository, CancellationToken ct)
    {
        return new()
        {
            Id = inquire.Id,
            MoneyInSmallestUnit = inquire.MoneyInSmallestUnit,
            NumberOfInstallments = inquire.NumberOfInstallments,
            CreationTime = inquire.CreationTime,
            DbPersonalData = await GetPersonalDataAsync(inquire, usersRepository, ct),
        };
    }

    private async Task<DbPersonalData> GetPersonalDataAsync(Inquire inquire, UsersRepository usersRepository, CancellationToken ct)
    {
        if (inquire.PersonalData is not null)
        {
            return inquire.PersonalData.ToDbPersonalData();
        }

        var user = await usersRepository.FindAndEnsureExistence(inquire.UserId!, ct);
        return user.PersonalData.ToDbPersonalData();
    }
}
