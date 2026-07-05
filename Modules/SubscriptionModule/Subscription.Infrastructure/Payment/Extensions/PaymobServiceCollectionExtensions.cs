using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Subscription.Application.Contracts.Infrastructure;
using Subscription.Application.DTOs.PaymentDtos;
using Subscription.Infrastructure.Payment.Helper;
using Subscription.Infrastructure.Payment.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Subscription.Infrastructure.Payment.Extensions
{
    public static class PaymobHttpClientNames
    {
        public const string PaymobClient = "PaymobClient";
    }

    public static class PaymobServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymobPayments(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<PaymobOptions>()
                .Bind(configuration.GetSection(PaymobOptions.SectionName))
                .ValidateDataAnnotations()
                .Validate(o => !string.IsNullOrWhiteSpace(o.SecretKey), "Paymob:SecretKey is required.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.PublicKey), "Paymob:PublicKey is required.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.IntegrationId), "Paymob:IntegrationId is required.")
                .Validate(o => !string.IsNullOrWhiteSpace(o.Hmac), "Paymob:Hmac is required.")
                .ValidateOnStart();

            services.AddHttpClient(PaymobHttpClientNames.PaymobClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<PaymobOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddScoped<IPaymobPaymentService, PaymobPaymentService>();
            services.AddScoped<IHmacService, HmacService>();

            return services;
        }
    }
}
