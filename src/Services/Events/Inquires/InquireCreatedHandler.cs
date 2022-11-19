using Domain.Inquires;
using Domain.Offers;
using FastEndpoints;
using Services.Data;
using Services.Data.Repositories;

namespace Services.Events.Inquires;

public class InquireCreatedHandler : IEventHandler<InquireCreatedEvent>
{
    private readonly Repository<Inquire> inquiriesRepository;
    private readonly Repository<Offer> offersRepository;
    private readonly CoreDbContext dbContext;

    public InquireCreatedHandler(Repository<Inquire> inquiriesRepository, Repository<Offer> offersRepository, CoreDbContext dbContext)
    {
        this.inquiriesRepository = inquiriesRepository;
        this.offersRepository = offersRepository;
        this.dbContext = dbContext;
    }

    public async Task HandleAsync(InquireCreatedEvent eventModel, CancellationToken ct)
    {
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
