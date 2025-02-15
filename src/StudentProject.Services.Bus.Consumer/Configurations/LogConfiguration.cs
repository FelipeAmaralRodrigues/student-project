using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace StudentProject.Services.Bus.Consumer.Configurations
{
    public static class LogConfiguration
    {
        public static IHostBuilder AddLogConfiguration(this IHostBuilder host)
        {
            host.UseSerilog((host, log) =>
            {
                if (host.HostingEnvironment.IsProduction())
                    log.MinimumLevel.Information();
                else
                    log.MinimumLevel.Debug();

                log.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
                log.MinimumLevel.Override("Masstransit", LogEventLevel.Information);
                log.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information);
                log.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);
                log.WriteTo.Console();
            });

            return host;
        }
    }
}
