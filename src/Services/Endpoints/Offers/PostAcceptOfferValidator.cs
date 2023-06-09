using Contracts.Offers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferValidator: Validator<PostAcceptOffer>
{
    public PostAcceptOfferValidator()
    {
        RuleFor(req => req.Contract)
            .NotEmpty()
            .WithErrorCode(PostAcceptOffer.ErrorCodes.FileCanNotBeEmpty.ToString());
        RuleFor(req => req.Contract.Length)
            .LessThan(16 * 1024 * 1024)
            .WithErrorCode(PostAcceptOffer.ErrorCodes.FileHasToBeSmallerThan16MB.ToString());
        RuleFor(req => req.OfferId)
            .Must(GuidCheck)
            .WithErrorCode(PostAcceptOffer.ErrorCodes.OfferDoesNotExist.ToString());

    }

    private bool GuidCheck(Guid offerId)
    {
        var dbContext = Resolve<CoreDbContext>();
        var offer = dbContext.Offers.Find(offerId);
        return offer != null;
    }
}