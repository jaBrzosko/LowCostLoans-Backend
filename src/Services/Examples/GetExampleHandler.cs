using Contracts.Examples;

namespace Services.Examples;

public class GetExampleHandler : IHttpGetHandler<GetExample, ExampleDto>
{
    public Task<ExampleDto> GetAsync(GetExample args)
    {
        return Task.FromResult(new ExampleDto
        {
            CreationTime = DateTime.Now,
            Id = 1,
            Name = "ala ma kota",
        });
    }
}
