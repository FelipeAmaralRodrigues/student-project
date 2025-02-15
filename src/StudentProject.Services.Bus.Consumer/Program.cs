using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StudentProject.Services.Api.Configurations;
using StudentProject.Services.Bus.Consumer.Configurations;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
    .AddCommandLine(args)
    .AddEnvironmentVariables()
.Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.Sources.Clear();
        builder.AddConfiguration(configuration);
    })
    .AddLogConfiguration()
    .ConfigureServices((hostContext, services) => 
    {
        services.AddDataConfigurations();
        services.AddBusConfiguration(configuration);
        services.ResolveDependencies(configuration);
    }).Build();

await host.RunAsync();
