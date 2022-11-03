namespace Services;

public interface IHttpGetHandler<GetType, ReturnedType>
{
    public Task<ReturnedType> GetAsync(GetType args);
}
