using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Subscription;
using SharedKernel.Website;
using Subscription.Application.Contracts.Payment;
using Subscription.Application.Features.Payments.Services;
using Subscription.Application.Features.Payments.Strategies;
using Subscription.Application.Features.Payments.Validators;
using Subscription.Application.Services;
using Subscription.Domain.Enums;
using System.Reflection;

namespace Subscription.Application.DependencyInjection
{
    public static class SubscriptionApplicationDependencyInjection
    {
        public static IServiceCollection AddSubscriptionApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            // Shared payment initiation orchestration
            services.AddScoped<IPaymentInitiationService, PaymentInitiationService>();

            // Implement SharedKernel contracts for cross-module consumption
            services.AddScoped<ISubscriptionModuleChecker, SubscriptionModuleChecker>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IPermissionModuleMapper, PermissionModuleMapper>();

            // Module add-on services
            services.AddScoped<IEffectiveModuleService, EffectiveModuleService>();
            services.AddScoped<IModulePurchaseService, ModulePurchaseService>();

            // Implement SharedKernel payment contracts for Website module
            services.AddScoped<IOrderPaymentService, OrderPaymentService>();

            // Keyed Validators (resolved by PaymentPurpose in InitiatePaymentCommandHandler)
            services.AddKeyedScoped<IPaymentInitiationValidator, ModulePurchaseInitiationValidator>(PaymentPurpose.ModulePurchase);
            services.AddKeyedScoped<IPaymentInitiationValidator, SubscriptionRenewalInitiationValidator>(PaymentPurpose.SubscriptionRenewal);
            services.AddKeyedScoped<IPaymentInitiationValidator, ModuleRenewalInitiationValidator>(PaymentPurpose.ModuleRenewal);
            services.AddKeyedScoped<IPaymentInitiationValidator, WebsiteOrderInitiationValidator>(PaymentPurpose.WebsiteOrder);

            // Keyed Strategies (resolved by PaymentPurpose in ProcessWebhookCommandHandler)
            services.AddKeyedScoped<IPaymentCompletionStrategy, CreateCompanyCompletionStrategy>(PaymentPurpose.CreateCompany);
            services.AddKeyedScoped<IPaymentCompletionStrategy, CompleteModulePurchaseStrategy>(PaymentPurpose.ModulePurchase);
            services.AddKeyedScoped<IPaymentCompletionStrategy, CompleteSubscriptionRenewalStrategy>(PaymentPurpose.SubscriptionRenewal);
            services.AddKeyedScoped<IPaymentCompletionStrategy, CompleteModuleRenewalStrategy>(PaymentPurpose.ModuleRenewal);
            services.AddKeyedScoped<IPaymentCompletionStrategy, CompleteWebsiteOrderStrategy>(PaymentPurpose.WebsiteOrder);

            return services;
        }
    }
}

