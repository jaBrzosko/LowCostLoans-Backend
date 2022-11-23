using Domain.Examples;
using Domain.Inquires;
using Domain.Offers;
using FastEndpoints;
using FastEndpoints.Swagger;
using Services.Data;
using Services.Data.Repositories;
using Services.ValidationExtensions;
using Services.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Services.Data.Auth0;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<CoreDbContext>();
        builder.Services.AddHttpClient<Auth0Client>().ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Auth0ApiUrl"]);
        });
        builder.Services.AddScoped<ExampleService>();
        builder.Services.AddScoped<Repository<Example>>();
        builder.Services.AddScoped<Repository<Inquire>>();
        builder.Services.AddScoped<Repository<Offer>>();
        builder.Services.AddFastEndpoints();
        builder.Services.AddSwaggerDoc();
        // CORS
        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(host => host.StartsWith(builder.Configuration["FrontendPrefix"]))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }));

        // Auth0
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["JwtAuthority"];
            options.Audience = builder.Configuration["JwtAudience"];
        });

        var app = builder.Build();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints(c =>
        {
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
            {
                return new ValidationErrors
                {
                    StatusCode = statusCode,
                    Errors = failures
                        .Select(f => new Error
                        {
                            ErrorCode = int.Parse(f.ErrorCode),
                            ErrorMessage = f.ErrorMessage,
                        })
                        .ToList(),
                };
            };
        });
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());

        app.Run();
    }
}
