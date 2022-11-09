using Contracts.Examples;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Services;
using Services.Services;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<ExampleService>();
        builder.Services.AddFastEndpoints();
        builder.Services.AddSwaggerDoc();
        var app = builder.Build();

        app.UseAuthorization();
        app.UseFastEndpoints();
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());

        app.Run();
    }
}
