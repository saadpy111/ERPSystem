using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Infrastructure.Payment.Extensions;
using Subscription.Infrastructure.Payment.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Infrastructure.Payment.PaymentService
{
    public sealed class PaymobPaymentService : IPaymobPaymentService
    {
        private const string CreateIntentionPath = "v1/intention/";
        private const string UnifiedCheckoutPath = "unifiedcheckout/";

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        private readonly HttpClient _httpClient;
        private readonly PaymobOptions _options;
        private readonly ILogger<PaymobPaymentService> _logger;

        public PaymobPaymentService(
            IHttpClientFactory httpClientFactory,
            IOptions<PaymobOptions> options,
            ILogger<PaymobPaymentService> logger)
        {
            _httpClient = httpClientFactory.CreateClient(PaymobHttpClientNames.PaymobClient);
            _options = options.Value;
            _logger = logger;
        }

        public async Task<PaymentIntentionResult> CreatePaymentIntentionAsync(
            CreatePaymentIntentionCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            var requestPayload = BuildIntentionRequest(command);

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, CreateIntentionPath)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestPayload, SerializerOptions),
                    Encoding.UTF8,
                    "application/json")
            };
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Token", _options.SecretKey);

            _logger.LogInformation(
                "Creating Paymob payment intention for PaymentId {PaymentId}, TenantId {TenantId}, Amount {Amount}.",
                command.PaymentId,
                command.TenantId,
                command.Amount);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while calling Paymob Create Intention API.");
                throw new PaymobApiException("Failed to reach the Paymob payment gateway.", ex);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Timeout while calling Paymob Create Intention API.");
                throw new PaymobApiException("Timed out while contacting the Paymob payment gateway.", ex);
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Paymob Create Intention API returned {StatusCode}. Body: {Body}",
                    (int)response.StatusCode,
                    responseBody);

                throw new PaymobApiException(
                    responseBody,
                    responseBody,
                    (int)response.StatusCode);
            }

            PaymentIntentionResponse? intentionResponse;
            try
            {
                intentionResponse = JsonSerializer.Deserialize<PaymentIntentionResponse>(responseBody, SerializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize Paymob Create Intention response. Body: {Body}", responseBody);
                throw new PaymobApiException("Received an unexpected response from Paymob.", ex);
            }

            if (intentionResponse is null || string.IsNullOrWhiteSpace(intentionResponse.ClientSecret))
            {
                _logger.LogError("Paymob Create Intention response missing client_secret. Body: {Body}", responseBody);
                throw new PaymobApiException("Paymob did not return a valid client secret.");
            }

            var checkoutUrl =
                $"{_options.BaseUrl.TrimEnd('/')}/{UnifiedCheckoutPath}?publicKey={Uri.EscapeDataString(_options.PublicKey)}&clientSecret={Uri.EscapeDataString(intentionResponse.ClientSecret)}";

            return new PaymentIntentionResult(
                PaymentId: intentionResponse.Id,
                ClientSecret: intentionResponse.ClientSecret,
                CheckoutUrl: checkoutUrl,
                ReferenceId: command.PaymentId,
                IntentionOrderId: intentionResponse.IntentionOrderId,
                Status: intentionResponse.Status);
        }

        private PaymentIntentionRequest BuildIntentionRequest(CreatePaymentIntentionCommand command)
        {
            var amountCents = (int)Math.Round(command.Amount * 100m, MidpointRounding.AwayFromZero);

            return new PaymentIntentionRequest
            {
                Amount = amountCents,
                Currency = command.CurrencyCode,
                PaymentMethods = new List<object> { ParseIntegrationId(_options.IntegrationId) },
                Items = new List<PaymentIntentionItem>
                {
                    new()
                    {
                        Name = command.ItemName,
                        Amount = amountCents,
                        Description = command.ItemDescription,
                        Quantity = 1
                    }
                },
                BillingData = new BillingData
                {
                    FirstName = command.CustomerFirstName,
                    LastName = command.CustomerLastName,
                    Email = command.CustomerEmail,
                    PhoneNumber = command.CustomerPhone
                },
                Extras = new Extras
                {
                    TenantId = command.TenantId
                },
                SpecialReference = command.PaymentId,
                Expiration = _options.ExpirationSeconds,
                NotificationUrl = string.IsNullOrWhiteSpace(_options.NotificationUrl) ? null : _options.NotificationUrl,
                RedirectionUrl = string.IsNullOrWhiteSpace(_options.RedirectionUrl) ? null : _options.RedirectionUrl
            };
        }

        private static object ParseIntegrationId(string integrationId)
        {
            return long.TryParse(integrationId, out var numericId) ? numericId : integrationId;
        }
    }
}
