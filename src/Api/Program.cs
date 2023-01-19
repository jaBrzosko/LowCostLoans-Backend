using FastEndpoints;
using FastEndpoints.Swagger;
using Services.Data;
using Services.Data.Repositories;
using Services.ValidationExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Services.Configurations;
using Services.Services.Apis.LoanBankApis;
using Services.Services.Apis.LoanBankApis.Clients;
using Services.Services.Apis.OurApis;
using Services.Services.Apis.OurApis.Clients;
using Services.Services.Auth0;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts.UseNpgsql(builder.Configuration["DatabaseConnectionString"])
        );

        builder.Services.AddSingleton(new OurApiConfiguration(builder.Configuration["OurApiUrlPrefix"], builder.Configuration["OurApiApiKey"]));
        builder.Services.AddSingleton(new Auth0Configuration(builder.Configuration["Auth0ApiUrl"]));
        builder.Services.AddSingleton(new LoanBankConfiguration(builder.Configuration["LoanBankUrlPrefix"], 
            builder.Configuration["LoanBankAuthUrlPrefix"], builder.Configuration["LoanBankApiKeyName"], 
            builder.Configuration["LoanBankApiKeySecret"]));

        builder.Services.AddHttpClient<Auth0Client>().ConfigureHttpClient(Auth0Client.Configure);
        builder.Services.AddHttpClient<OurApiClient>().ConfigureHttpClient(OurApiClient.Configure);
        builder.Services.AddHttpClient<LoanBankClient>().ConfigureHttpClient(LoanBankClient.Configure);
        builder.Services.AddHttpClient<LoanBankAuthClient>().ConfigureHttpClient(LoanBankAuthClient.Configure);
        
        builder.Services.AddScoped<InquiresRepository>();
        builder.Services.AddScoped<OffersRepository>();
        builder.Services.AddScoped<UsersRepository>();
        
        builder.Services.AddScoped<OurApiOffersGetter>();
        builder.Services.AddScoped<LoanBankOffersGetter>();

        builder.Services.AddScoped<LoanBankInquireResolver>();
        
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
                            ErrorCode = 0,
                            ErrorMessage = f.ErrorMessage,
                        })
                        .ToList(),
                };
            };
        });
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
        
            var context = services.GetRequiredService<CoreDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        app.Run();
    }
}
