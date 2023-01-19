using Contracts.Users;
using Domain.Users;

namespace Services.Data.DataMappers;

public static class UserDataMapper
{
    public static PersonalData? ToNullableEntity(this PersonalDataDto? personalDataDto)
    {
        if (personalDataDto is null)
        {
            return null;
        }

        return ToEntity(personalDataDto);
    }
    
    public static PersonalData ToEntity(this PersonalDataDto personalDataDto)
    {
        return new PersonalData(
            personalDataDto.FirstName,
            personalDataDto.LastName,
            personalDataDto.GovernmentId,
            personalDataDto.Email,
            (GovernmentIdType)personalDataDto.GovernmentIdType,
            (JobType)personalDataDto.JobType
        );
    }

    public static PersonalDataDto ToDto(this PersonalData personalData)
    {
        return new()
        {
            FirstName = personalData.FirstName,
            LastName = personalData.LastName,
            GovernmentId = personalData.GovernmentId,
            Email = personalData.Email,
            GovernmentIdType = (GovernmentIdTypeDto)personalData.GovernmentIdType,
            JobType = (JobTypeDto)personalData.JobType,
        };
    }

    public static PersonalDataDto? ToNullableDto(this PersonalData? personalData)
    {
        if (personalData is null)
        {
            return null;
        }

        return ToDto(personalData);
    }
}
