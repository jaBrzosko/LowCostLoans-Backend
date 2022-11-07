namespace Domain.Examples;

public class Example
{
    public int Id { get; private init; }
    public string Name { get; private set;  }
    public DateTime CreationTime { get; private init;  }

    public Example(int id, string name)
    {
        Id = id;
        Name = name;
        CreationTime = DateTime.Now;
    }
}
