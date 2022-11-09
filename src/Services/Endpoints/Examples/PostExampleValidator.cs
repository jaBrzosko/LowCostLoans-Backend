using Contracts.Examples;
using FastEndpoints;
using FluentValidation;
using Services.ValidationExtensions;

namespace Services.Endpoints.Examples;

public class PostExampleValidator : Validator<PostExample>
{
    public PostExampleValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithErrorCode(PostExample.ErrorCodes.NameIsEmpty)
            .MaximumLength(20)
            .WithErrorCode(PostExample.ErrorCodes.NameIsTooLong);
    }
}
