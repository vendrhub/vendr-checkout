#if NET

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Vendr.Checkout.Configuration;
using Vendr.Checkout.Events;
using Vendr.Checkout.Extensions;
using Vendr.Checkout.Services;

namespace Vendr.Checkout
{
    public static class VendrCheckoutUmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddVendrCheckout(this IUmbracoBuilder builder, Action<VendrCheckoutSettings> defaultOptions = default)
        {
            // If the Vendr Checkout InstallService is registred then we assume everything is already registered so we don't do it again. 
            if (builder.Services.FirstOrDefault(x => x.ServiceType == typeof(InstallService)) != null)
                return builder;

            // Register configuration
            var options = builder.Services.AddOptions<VendrCheckoutSettings>()
                .Bind(builder.Config.GetSection(VendrCheckoutConstants.System.ProductName));

            if (defaultOptions != default)
                options.Configure(defaultOptions);

            options.ValidateDataAnnotations();

            // Register event handlers
            builder.AddVendrEventHandlers();

            // Register pipeline
            builder.AddVendrInstallPipeline();

            // Register services
            builder.Services.AddSingleton<InstallService>();

            // Register helpers
            builder.Services.AddSingleton<PathHelper>();

            // Register Umbraco event handlers
            builder.AddNotificationHandler<ContentCacheRefresherNotification, SyncZeroValuePaymentProviderContinueUrl>();

            return builder;
        }
    }
}

#endif