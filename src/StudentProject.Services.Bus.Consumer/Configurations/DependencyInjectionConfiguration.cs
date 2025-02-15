using MediatR;
using Serilog;
using StudentProject.Domain.Mediator.Notifications;
using StudentProject.Domain.Mediator;
using StudentProject.Domain.Data;
using StudentProject.Domain.Students.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StudentProject.Infra.Data.Repositories;
using StudentProject.Infra.Data.Data;

namespace StudentProject.Services.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyMediatrProject = AppDomain.CurrentDomain.Load("StudentProject.Domain");
            services.AddMediatR(o =>
            {
                o.RegisterServicesFromAssembly(assemblyMediatrProject);
                o.AddPipelineValidator(services, assemblyMediatrProject);
            });

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // loggers
            services.AddLogging(builder => builder.AddSerilog());
        }
    }
}