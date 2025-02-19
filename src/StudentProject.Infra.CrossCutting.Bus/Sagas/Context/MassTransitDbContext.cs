using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentProject.Contracts;
using StudentProject.Infra.CrossCutting.Bus.Sagas.Mapping;

namespace StudentProject.Infra.CrossCutting.Bus.Sagas.Context
{
    public class MassTransitDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MassTransitDbContext(
            DbContextOptions<MassTransitDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<StudentCreatedThirdPartyRegistrationSagaData> StudentCreatedThirdPartyRegistrationSagaData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new StudentCreatedThirdPartyRegistrationSagaDataMapping());
            builder.HasDefaultSchema("mst");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder
                    .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                    .UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"], sql =>
                    {
                        sql.MigrationsHistoryTable("__EFMigrationsHistory", "mst");
                        sql.EnableRetryOnFailure();
                    });
            }
        }
    }
}
