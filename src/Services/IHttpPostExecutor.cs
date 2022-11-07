namespace Services;

public interface IHttpPostExecutor<TPost>
{
    public Task HandleAsync(TPost args);
}
