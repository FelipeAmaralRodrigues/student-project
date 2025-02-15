using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;
using StudentProject.Infra.Data.Context;

namespace StudentProject.Services.Api.Configurations
{
    public static class HealthConfiguration
    {
        public static void AddHealthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!)
                .AddDbContextCheck<ApplicationDbContext>(tags: new[] { "appdbcontext", "ready" });
        }

        public static void UseHealthConfig(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = WriteResponse
            });
            app.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("ready")
            });
        }

        public static Task WriteResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions { Indented = true };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteStartObject("results");

                var mapping = new Dictionary<string, string>
                {
                    {"sqlserver", "db" },
                    {"ApplicationDbContext", "appdbcontext" },
                    {"masstransit-bus", "masstransit" }
                };

                foreach (var healthReportEntry in healthReport.Entries)
                {
                    string displayName;
                    if (!mapping.TryGetValue(healthReportEntry.Key, out displayName))
                        displayName = healthReportEntry.Key;
                    jsonWriter.WriteStartObject(displayName);
                    jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }
}
