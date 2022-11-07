using Contracts;
using Contracts.Examples;

namespace Services;

public class PostExampleExecutor : IHttpPostExecutor<PostExample>
{
    public Task HandleAsync(PostExample args)
    {
        return Task.CompletedTask;
    }
}
