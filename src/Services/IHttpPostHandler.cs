namespace Services;

public interface IHttpPostHandler<TPost>
{
    public Task HandleAsync(TPost args);
}
