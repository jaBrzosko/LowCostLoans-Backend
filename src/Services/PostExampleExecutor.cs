using Contracts;

namespace Services;

public class PostExampleExecutor : IHttpPostExecutor<PostExample>
{
    public Task HandleAsync(PostExample args)
    {
        return Task.CompletedTask;
    }
}
