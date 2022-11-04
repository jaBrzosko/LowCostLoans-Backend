using Contracts;

namespace Services;

public class PostExampleHandler : IHttpPostHandler<PostExample>
{
    public Task HandleAsync(PostExample args)
    {
        return Task.CompletedTask;
    }
}
