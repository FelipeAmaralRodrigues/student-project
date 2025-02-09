using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SagaExampleMassTransit.Domain.Entities;
using SagaExampleMassTransit.Infra.Data.Mappings;

namespace SagaExampleMassTransit.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new StudentMapping());
            builder.HasDefaultSchema("dbo");
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
                        sql.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                        sql.EnableRetryOnFailure();
                    });
            }
        }
    }
}
