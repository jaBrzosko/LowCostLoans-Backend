using Domain.Inquires;
using Domain.Offers;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Services.Data;
using Services.Data.Repositories;
using Services.Services.Apis;
using Services.Services.Apis.OurApis;
using Inquire = Domain.Inquires.Inquire;

namespace Services.Events.Inquires;

public class InquireCreatedHandler : IEventHandler<InquireCreatedEvent>
{
    private readonly IServiceProvider serviceProvider;
    private readonly CoreDbContext dbContext;
    private List<IApiOffersGetter> apiOffersGetters = null!;

    public InquireCreatedHandler(IServiceProvider serviceProvider, CoreDbContext dbContext)
    {
        this.serviceProvider = serviceProvider;
        this.dbContext = dbContext;
    }

    public async Task HandleAsync(InquireCreatedEvent eventModel, CancellationToken ct)
    {
        ConstructApiOffersGetters();
        var inquiriesRepository = serviceProvider.GetService<InquiresRepository>()!;
        var offersRepository = serviceProvider.GetService<OffersRepository>()!;

        var inquire = await inquiriesRepository.FindAndEnsureExistence(eventModel.InquireId, ct);

        try
        {
            foreach (var offersGetter in apiOffersGetters)
            {
                var apiOffers = await offersGetter.GetOffersAsync(ToDbData(inquire), ct);
                foreach (var o in apiOffers)
                {
                    var offer = new Offer(inquire.Id, o.InterestRateInPromiles, o.MoneyInSmallestUnit, o.NumberOfInstallments);
                    offersRepository.Add(offer);
                }
            }

            inquire.UpdateStatus(InquireStatus.OffersGenerated);
            inquiriesRepository.Update(inquire);
        }
        catch (Exception)
        {
            inquire.UpdateStatus(InquireStatus.OffersGenerationFailed);
            inquiriesRepository.Update(inquire);
        }

        await dbContext.SaveChangesAsync(ct);
    }

    private void ConstructApiOffersGetters()
    {
        apiOffersGetters = new()
        {
            serviceProvider.GetService<OurApiOffersGetter>()!,
        };
    }

    private DbInquireData ToDbData(Inquire inquire)
    {
        return new()
        {
            Id = inquire.Id,
            MoneyInSmallestUnit = inquire.MoneyInSmallestUnit,
            NumberOfInstallments = inquire.NumberOfInstallments,
            CreationTime = inquire.CreationTime,
        };
    }
}
