using Contracts.Users;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Users;

public class PostRegisterUserValidator : Validator<PostRegisterUser>
{
    public PostRegisterUserValidator()
    {
        RuleFor(ru => ru.UserId)
            .MustAsync(IsIdAvailable)
            .WithMessage("This User is already registered")
            .WithErrorCode(PostRegisterUser.ErrorCodes.UserAlreadyRegistered)
            .NotEmpty()
            .WithErrorCode(PostRegisterUser.ErrorCodes.UserIdIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithErrorCode(PostRegisterUser.ErrorCodes.UserIdIsTooLong);
            
        RuleFor(ru => ru.PersonalData)
            .SetValidator(new PersonalDataValidator());
    }

    private Task<bool> IsIdAvailable(string userId, CancellationToken ct)
    {
        return Resolve<CoreDbContext>()
            .Users
            .AllAsync(u => u.Id != userId, ct);
    }
}
