namespace Domain.Users;

public class User : IDbEntity
{
    public Guid Id { get; private init; }
    public PersonalData PersonalData { get; private set; }

    public User(PersonalData personalData)
    {
        Id = Guid.NewGuid();
        PersonalData = personalData;
    }
    
    private User()
    { }
}
