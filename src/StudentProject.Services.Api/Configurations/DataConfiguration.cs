using StudentProject.Domain.Data;
using StudentProject.Domain.Students.Repositories;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Context;
using StudentProject.Infra.Data.Context;
using StudentProject.Infra.Data.Data;
using StudentProject.Infra.Data.Repositories;

namespace StudentProject.Services.Api.Configurations
{
    public static class DataConfiguration
    {
        public static void AddDataConfigurations(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<MassTransitDbContext>();

            // repositories read only
            services.AddScoped<IStudentReadOnlyRepository, StudentReadOnlyRepository>();

            // repositories
            services.AddScoped<IStudentRepository, StudentRepository>();

            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
