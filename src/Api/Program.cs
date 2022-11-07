using Contracts;
using Services;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<GetExampleHandler>();
        builder.Services.AddScoped<IHttpPostExecutor<PostExample>, PostExampleExecutor>();
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");
        app.MapGet("/Example", (int id, string name, GetExampleHandler handler) => handler.GetAsync((id, name)));
        app.MapPost("/Example", (PostExample request, IHttpPostExecutor<PostExample> handler) => handler.HandleAsync(request));

        app.Run();
    }
}
