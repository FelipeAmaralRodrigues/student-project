using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentProject.Contracts;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Context;

namespace StudentProject.Services.Bus.Consumer.Configurations
{
    public static class BusConfiguration
    {
        public static void AddBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // bus
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumersFromNamespaceContaining(typeof(BusNamespace));

                x.AddConsumer<CreateStudentConsumer>();
                x.AddConsumer<SendRequestCreateStudentThirdPartyUIdConsumer>();
                x.AddConsumer<ReceiveResponseCreateStudentThirdPartyUIdConsumer>();
                x.AddConsumer<UpdateStudentThirdPartyUIdConsumer>();

                x.AddDelayedMessageScheduler();

                x.AddSagaStateMachine<StudentCreatedThirdPartyRegistrationSaga, StudentCreatedThirdPartyRegistrationSagaData>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<MassTransitDbContext>();
                    });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration.GetConnectionString("BusConnection")));
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}

