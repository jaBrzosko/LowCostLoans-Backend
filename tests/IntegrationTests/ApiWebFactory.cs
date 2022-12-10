using Api;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IntegrationTests.MockedServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Services.Data;
using Services.Data.Auth0;

using Services.Services.Apis.OurApis.Clients;
using Xunit;

namespace IntegrationTests;

public class ApiWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase database = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "testDb",
            Username = "testUser",
            Password = "doesnt_matter",
        }).Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());
        
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<CoreDbContext>));
            services.Remove(descriptor);

            services.RemoveAll<CoreDbContext>();
            services.AddDbContext<CoreDbContext>(
                opts => opts.UseNpgsql(database.ConnectionString)
            );


            services.RemoveAll<OurApiClient>();
            services.RemoveAll<Auth0Client>();
            services.AddHttpClient<OurApiClient, MockedOurApiClient>();
            services.AddHttpClient<Auth0Client, MockedAuth0Client>();
        });
    }
    
    public async Task InitializeAsync()
    {
        await database.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await database.DisposeAsync();
    }
}
