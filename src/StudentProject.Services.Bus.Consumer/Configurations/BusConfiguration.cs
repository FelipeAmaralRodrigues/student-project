using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Context;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Datas;
using StudentProject.Infra.Data.Context;
using StudentProject.Services.Bus.Consumer.Consumers;
using StudentProject.Services.Worker.Sagas;

namespace StudentProject.Services.Bus.Consumer.Configurations
{
    public static class BusConfiguration
    {
        public static void AddBusConfiguration(this  IServiceCollection services, IConfiguration configuration)
        {
            // bus
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
             
                x.AddConsumer<CreateStudentConsumer>();
                x.AddConsumer<SendRequestCreateStudentThirdPartyUIdConsumer>();
                x.AddConsumer<ReceiveResponseCreateStudentThirdPartyUIdConsumer>();
                x.AddConsumer<UpdateStudentThirdPartyUIdConsumer>();

                x.AddSagaStateMachine<StudentCreatedThirdPartyRegistrationSaga, StudentCreatedThirdPartyRegistrationSagaData>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<MassTransitDbContext>();
                    });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Uri"], "/", c =>
                    {
                        c.Username(configuration["RabbitMq:Usr"]);
                        c.Password(configuration["RabbitMq:Pwd"]);
                    });

                    cfg.ReceiveEndpoint("std-create-student", c =>
                    {
                        c.Consumer<CreateStudentConsumer>(context);
                        c.UseRateLimit(10, TimeSpan.FromMinutes(2));
                        c.UseMessageRetry(r => r.None());
                    });

                    cfg.ReceiveEndpoint("std-send-request-create-student-third-party-uid", c =>
                    {
                        c.Consumer<SendRequestCreateStudentThirdPartyUIdConsumer>(context);
                        c.UseRateLimit(10, TimeSpan.FromMinutes(2));
                        c.UseMessageRetry(r => r.None());
                    });

                    cfg.ReceiveEndpoint("std-receive-response-create-student-third-party-uid", c =>
                    {
                        c.Consumer<ReceiveResponseCreateStudentThirdPartyUIdConsumer>(context);
                        c.UseRateLimit(10, TimeSpan.FromMinutes(2));
                        c.UseMessageRetry(r => r.None());
                    });

                    cfg.ReceiveEndpoint("std-update-student-third-party-uid", c =>
                    {
                        c.Consumer<UpdateStudentThirdPartyUIdConsumer>(context);
                        c.UseRateLimit(10, TimeSpan.FromMinutes(2));
                        c.UseMessageRetry(r => r.None());
                    });
                });
            });
        }
    }
}
