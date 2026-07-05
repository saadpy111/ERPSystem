using MediatR;
using Subscription.Application.DTOs.PaymentDtos;

namespace Subscription.Application.Features.Payments.Commands.ProcessWebhook
{
    public class ProcessWebhookCommand : IRequest<ProcessWebhookResponse>
    {
        public string Hmac { get; }
        public WebhookDto Webhook { get; }

        public ProcessWebhookCommand(string hmac, WebhookDto webhook)
        {
            Hmac = hmac;
            Webhook = webhook;
        }
    }

    public class ProcessWebhookResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
