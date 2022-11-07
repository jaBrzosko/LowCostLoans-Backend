using Contracts.Examples;
using FluentValidation;
using Services;
using Services.Examples;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<GetExampleHandler>();
        builder.Services.AddScoped<IHttpPostExecutor<PostExample>, PostExampleExecutor>();
        builder.Services.AddScoped<AbstractValidator<PostExample>, PostExampleValidator>();
        builder.Services.AddScoped<HttpPostHandler<PostExample>>();
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapGet("/Example", (int id, string name, GetExampleHandler handler) => handler.GetAsync((id, name)));
        app.MapPost("/Example", (PostExample request, HttpPostHandler<PostExample> handler) => handler.HandleAsync(request));

        app.Run();
    }
}
