using Contracts;

namespace Services;

public interface IHttpGetHandler<TIn, TOut> where TIn : IHttpGet<TOut>
{
    public Task<TOut> GetAsync(TIn args);
}
