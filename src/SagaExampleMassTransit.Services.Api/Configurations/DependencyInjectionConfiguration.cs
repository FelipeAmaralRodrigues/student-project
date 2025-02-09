using MediatR;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using SagaExampleMassTransit.Domain.Mediator.Notifications;
using SagaExampleMassTransit.Domain.Mediator;
using SagaExampleMassTransit.Services.Api.Filters;
using SagaExampleMassTransit.Infra.Data.Data;
using SagaExampleMassTransit.Domain.Data;
using SagaExampleMassTransit.Domain.Repositories;
using SagaExampleMassTransit.Infra.Data.Repositories;

namespace SagaExampleMassTransit.Services.Api.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyMediatrProject = AppDomain.CurrentDomain.Load("SagaExampleMassTransit.Domain");
            services.AddMediatR(o =>
            {
                o.RegisterServicesFromAssembly(assemblyMediatrProject);
                o.AddPipelineValidator(services, assemblyMediatrProject);
            });

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // loggers
            services.AddLogging(builder => builder.AddSerilog());
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();

            // repositories read only

            // repositories
            services.AddScoped<IStudentRepository, StudentRepository>();

            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // bus
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    var connectionString = configuration.GetConnectionString("BusConnection");
                    cfg.Host(connectionString);
                });
            });
        }
    }
}