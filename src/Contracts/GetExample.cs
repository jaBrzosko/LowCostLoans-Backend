﻿namespace Contracts;

public class GetExample
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static implicit operator GetExample((int Id, string Name) args) =>
        new GetExample { Id = args.Id, Name = args.Name };
}
