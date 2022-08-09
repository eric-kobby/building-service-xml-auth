using System.Net.Http.Headers;
using SoftwareDevelopmentTest;
using SoftwareDevelopmentTest.DAL;
using SoftwareDevelopmentTest.Models;
using SoftwareDevelopmentTest.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
        var configuration = hostingContext.Configuration;
        services.Configure<Credential>(configuration.GetSection("Credential"));
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddHttpClient<IBuildingService, BuildingService>(client =>
        {
            client.BaseAddress = new Uri(configuration["BaseUrl"], UriKind.Absolute);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddDbContextFactory<AppDbContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("Database");
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

