using Domain.Inquires;
using Domain.Offers;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Services.Data;
using Services.Data.Repositories;

namespace Services.Events.Inquires;

public class InquireCreatedHandler : IEventHandler<InquireCreatedEvent>
{
    private readonly IServiceProvider serviceProvider;
    private readonly CoreDbContext dbContext;

    public InquireCreatedHandler(IServiceProvider serviceProvider, CoreDbContext dbContext)
    {
        this.serviceProvider = serviceProvider;
        this.dbContext = dbContext;
    }

    public async Task HandleAsync(InquireCreatedEvent eventModel, CancellationToken ct)
    {
        var inquiriesRepository = serviceProvider.GetService<Repository<Inquire>>()!;
        var offersRepository = serviceProvider.GetService<Repository<Offer>>()!;
        
        var inquire = await inquiriesRepository.FindAndEnsureExistence(eventModel.InquireId, ct);
        
        // TODO: ask different apis for creating offers
        var offers = new List<Offer>()
        {
            new(inquire.Id, 1, 1000, 12),
            new(inquire.Id, 2, 1010, 12),
            new(inquire.Id, 3, 3400, 24),
        };
        foreach (var o in offers)
        {
            offersRepository.Add(o);
        }
        inquire.UpdateStatus(InquireStatus.OffersGenerated);
        inquiriesRepository.Update(inquire);

        await dbContext.SaveChangesAsync(ct);
    }
}
