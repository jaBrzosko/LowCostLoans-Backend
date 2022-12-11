using Contracts.Inquires;
using FastEndpoints;
using FluentValidation;
using Services.Endpoints.Users;
using Services.ValidationExtensions;

namespace Services.Endpoints.Inquires;

public class PostCreateInquireValidator : Validator<PostCreateInquire>
{
    public PostCreateInquireValidator()
    {
        RuleFor(iq => iq.MoneyInSmallestUnit)
            .GreaterThan(0)
            .WithErrorCode(PostCreateInquireAsAnonymous.ErrorCodes.MoneyHasToBePositive);
        
        RuleFor(iq => iq.NumberOfInstallments)
            .GreaterThan(0)
            .WithErrorCode(PostCreateInquireAsAnonymous.ErrorCodes.NumberOfInstallmentsHasToBePositive);
    }
}