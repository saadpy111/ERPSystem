using SharedKernel.Enums;
using Subscription.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Subscription.Application.DTOs.PaymentDtos
{
    public sealed record CreatePaymentIntentionCommand(
        string PaymentId,
        string TenantId,
        decimal Amount,
        string CurrencyCode,
        string ItemName,
        string ItemDescription,
        string CustomerFirstName,
        string CustomerLastName,
        string CustomerEmail,
        string CustomerPhone);

    public sealed record PaymentIntentionResult(
        string PaymentId,
        string ClientSecret,
        string CheckoutUrl,
        string ReferenceId,
        long IntentionOrderId,
        string Status);

    public sealed class PayRequest
    {

        [Required]
        public PaymentPurpose Purpose { get; set; }

        [Required]
        public string TargetId { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = "EGP";

        public BillingInterval Interval { get; set; } = BillingInterval.Monthly;

        [Required]
        [MaxLength(100)]
        public string CustomerFirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CustomerLastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;
    }

    public sealed record PayResponse(
        string PaymentId,
        string ClientSecret,
        string CheckoutUrl,
        string ReferenceId,
        string PublicKey,
        string Status,
        bool IsReused);

    public sealed record VerifyResponse(bool Success, string Message);
}
