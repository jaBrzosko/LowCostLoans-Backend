using Contracts.Users;
using FluentValidation;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Users;

public class PersonalDataValidator : AbstractValidator<PersonalDataDto>
{
    public PersonalDataValidator()
    {
        RuleFor(pd => pd.FirstName)
            .NotEmpty()
            .WithErrorCode(PersonalDataDto.ErrorCodes.FirstNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithErrorCode(PersonalDataDto.ErrorCodes.FirstNameIsTooLong);

        RuleFor(pd => pd.LastName)
            .NotEmpty()
            .WithErrorCode(PersonalDataDto.ErrorCodes.LastNameIsEmpty)
            .MaximumLength(StringLengths.ShortString)
            .WithErrorCode(PersonalDataDto.ErrorCodes.LastNameIsTooLong);

        RuleFor(pd => pd.GovernmentId)
            .NotEmpty()
            .WithErrorCode(PersonalDataDto.ErrorCodes.GovernmentIdIsEmpty)
            .MaximumLength(StringLengths.MediumString)
            .MaximumLength(PersonalDataDto.ErrorCodes.GovernmentIdIsTooLong);

        RuleFor(pd => pd)
            .Must(IsGovernmentIdValid)
            .WithErrorCode(PersonalDataDto.ErrorCodes.GovernmentIdIsInvalid)
            .WithMessage("GovernmentId is invalid");
    }

    private bool IsGovernmentIdValid(PersonalDataDto personalData)
    {
        switch (personalData.GovernmentIdType)
        {
            case GovernmentIdTypeDto.Pesel:
                return IsValidPesel(personalData.GovernmentId);
        }
        return false;
    }

    private bool IsValidPesel(string pesel)
    {
        return true;
    }
}