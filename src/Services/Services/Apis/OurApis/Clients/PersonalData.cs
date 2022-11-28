namespace Services.Services.Apis.OurApis.Clients;

internal class PersonalData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string GovernmentId { get; set; }
    public GovernmentIdTypeDto GovernmentIdType { get; set; }
    public JobTypeDto JobType { get; set; }
}

internal enum GovernmentIdTypeDto
{
    Pesel = 0,
}

internal enum JobTypeDto
{
    SomeJobType = 0,
}


