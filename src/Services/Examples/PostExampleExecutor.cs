using Contracts.Examples;

namespace Services.Examples;

public class PostExampleExecutor : IHttpPostExecutor<PostExample>
{
    public Task HandleAsync(PostExample args)
    {
        return Task.CompletedTask;
    }
}
