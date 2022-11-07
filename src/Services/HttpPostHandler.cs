using System.Net;
using Contracts;
using FluentValidation;

namespace Services;

public class HttpPostHandler<TIn> where TIn : IHttpPost
{
    private readonly AbstractValidator<TIn> validator;
    private readonly IHttpPostExecutor<TIn> executor;

    public HttpPostHandler(AbstractValidator<TIn> validator, IHttpPostExecutor<TIn> executor)
    {
        this.validator = validator;
        this.executor = executor;
    }

    public async Task<HttpResponseMessage> HandleAsync(TIn args)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(args);
            if (!validationResult.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.UnprocessableEntity);
            }

            await executor.HandleAsync(args);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}
