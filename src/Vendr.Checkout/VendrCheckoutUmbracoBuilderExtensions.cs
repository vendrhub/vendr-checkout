#if NET

using System;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Vendr.Checkout.Configuration;
using Vendr.Checkout.Extensions;
using Vendr.Checkout.Pipeline.Implement;
using Vendr.Checkout.Pipeline.Implement.Tasks;
using Vendr.Checkout.Services;
using Vendr.Extensions;

namespace Vendr.Checkout
{
    public static class VendrCheckoutUmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddVendrCheckout(this IUmbracoBuilder builder, Action<VendrCheckoutSettings> defaultOptions = default)
        {
            // If the Vendr IFactory is registred then we assume everything is already registered so we don't do it again. 
            //if (builder.Services.FirstOrDefault(x => x.ServiceType == typeof(IFactory)) != null)
            //    return builder;

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

            // TODO: SyncZeroValuePaymentProviderContinueUrl

            return builder;
        }
    }
}

#endif