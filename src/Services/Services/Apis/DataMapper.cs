namespace Services.Services.Apis;

public static class DataMapper
{
    public static DbPersonalData ToDbPersonalData(this Domain.Users.PersonalData domainPersonalData)
    {
        return new(
            domainPersonalData.FirstName,
            domainPersonalData.LastName,
            domainPersonalData.GovernmentId,
            (DbGovernmentIdType)domainPersonalData.GovernmentIdType,
            (DbJobType)domainPersonalData.JobType);
    }
}
