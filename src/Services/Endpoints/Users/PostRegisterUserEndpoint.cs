using Contracts.Users;
using Domain.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.DataMappers;
using Services.Data.Repositories;

namespace Services.Endpoints.Users;

[HttpPost("/users/registerUser")]
public class PostRegisterUserEndpoint : Endpoint<PostRegisterUser>
{
    private readonly UsersRepository usersRepository;

    public PostRegisterUserEndpoint(UsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }

    public override async Task HandleAsync(PostRegisterUser req, CancellationToken ct)
    {
        var user = new User(req.UserId, req.PersonalData.ToEntity());
        await usersRepository.AddAsync(user, ct);

        await SendAsync(new object(), cancellation: ct);
    }
}
