namespace Domain.Users;

public class User
{
    public string Id { get; private init; }
    public PersonalData PersonalData { get; private set; }

    public User(string id, PersonalData personalData)
    {
        Id = id;
        PersonalData = personalData;
    }
    
    private User()
    { }
}
