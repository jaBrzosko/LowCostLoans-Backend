using Contracts.Examples;
using FluentValidation;

namespace Services.Examples;

public class PostExampleValidator : AbstractValidator<PostExample>
{
    public PostExampleValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithErrorCode(PostExample.ErrorCodes.NameIsEmpty.ToString())
            .MaximumLength(20)
            .WithErrorCode(PostExample.ErrorCodes.NameIsTooLong.ToString());
    }
}
