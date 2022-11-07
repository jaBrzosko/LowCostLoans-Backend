using Contracts;

namespace Services;

public interface IHttpPostExecutor<TIn> where TIn : IHttpPost
{
    public Task HandleAsync(TIn args);
}
