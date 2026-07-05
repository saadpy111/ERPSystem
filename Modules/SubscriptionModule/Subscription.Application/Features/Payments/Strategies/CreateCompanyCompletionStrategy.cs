using Identity.Application.Features.TenantFeature.Commands.CreateCompany;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using Subscription.Application.Contracts.Payment;
using Subscription.Domain.Enums;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Application.Features.Payments.Strategies
{
    public class CreateCompanyCompletionStrategy : IPaymentCompletionStrategy
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateCompanyCompletionStrategy> _logger;

        public PaymentPurpose Purpose => PaymentPurpose.CreateCompany;

        public CreateCompanyCompletionStrategy(IMediator mediator, ILogger<CreateCompanyCompletionStrategy> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<PaymentCompletionResult> CompleteAsync(
            Subscription.Domain.Entities.Payment payment,
            Subscription.Domain.Entities.PaymentTransaction transaction,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(payment.Payload))
                {
                    _logger.LogError("CreateCompany payment {PaymentId} has no payload.", payment.Id);
                    return new PaymentCompletionResult(false, "Payment payload is missing.");
                }

                var payload = JsonSerializer.Deserialize<CompanyCreationPayload>(payment.Payload);
                if (payload == null)
                {
                    _logger.LogError("Failed to deserialize CreateCompany payload for payment {PaymentId}.", payment.Id);
                    return new PaymentCompletionResult(false, "Invalid payment payload.");
                }

                var command = new CreateCompanyCommand
                {
                    UserId = payload.UserId,
                    CompanyName = payload.CompanyName,
                    CompanyCode = payload.CompanyCode,
                    PlanCode = payload.PlanCode,
                    CurrencyCode = payload.CurrencyCode,
                    Interval = payload.Interval
                };

                var result = await _mediator.Send(command, cancellationToken);

                if (!result.Success)
                {
                    _logger.LogError("CreateCompanyCommand failed for payment {PaymentId}: {Error}", payment.Id, result.Error);
                    return new PaymentCompletionResult(false, result.Error);
                }

                _logger.LogInformation(
                    "Company created from payment {PaymentId}. TenantId={TenantId}, Company={CompanyCode}.",
                    payment.Id, result.TenantId, payload.CompanyCode);

                return new PaymentCompletionResult(true);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize CreateCompany payload for payment {PaymentId}.", payment.Id);
                return new PaymentCompletionResult(false, "Invalid payment payload format.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCompanyCompletionStrategy failed for payment {PaymentId}.", payment.Id);
                return new PaymentCompletionResult(false, $"Company creation failed: {ex.Message}");
            }
        }

        private sealed record CompanyCreationPayload(
            string CompanyName,
            string CompanyCode,
            string UserId,
            string PlanCode,
            string CurrencyCode,
            BillingInterval Interval);
    }
}
