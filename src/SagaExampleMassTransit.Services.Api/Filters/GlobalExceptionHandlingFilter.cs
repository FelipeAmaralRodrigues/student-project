using Microsoft.AspNetCore.Mvc.Filters;
namespace SagaExampleMassTransit.Services.Api.Filters
{
    public class GlobalExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;

        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(1, context.Exception, context.Exception.Message);
        }
    }
}