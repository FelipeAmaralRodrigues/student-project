using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SagaExampleMassTransit.Domain.Mediator.Notifications;

namespace SagaExampleMassTransit.Services.Api.Filters
{
    public class ValidateCommandFilter : IAsyncActionFilter
    {
        private readonly DomainNotificationHandler _notifications;

        public ValidateCommandFilter(INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.ContainsKey("command") && context.ActionArguments["command"] == null)
            {
                var cancellationToken = context.HttpContext.RequestAborted;

                await _notifications.Handle(new DomainNotification("Command", "Dados inválidos."), cancellationToken);

                context.Result = new BadRequestObjectResult(new Result
                {
                    Succeeded = false,
                    Errors = _notifications.GetNotifications().Select(n => new MessageError { Key = n.Key, Message = n.Value }).ToArray()
                });

                return;
            }

            await next();
        }
    }
}
