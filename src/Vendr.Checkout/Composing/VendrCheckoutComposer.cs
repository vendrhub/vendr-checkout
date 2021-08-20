using Vendr.Checkout.Extensions;
using Vendr.Checkout.Configuration;
using Vendr.Checkout.Services;
using Vendr.Checkout.Pipeline.Implement;

#if NETFRAMEWORK
using Umbraco.Core;
using Umbraco.Core.Composing;
using IBuilder = Umbraco.Core.Composing.Composition;
#else
using Umbraco.Cms.Core.Composing;
using IBuilder = Umbraco.Cms.Core.DependencyInjection.IUmbracoBuilder;
#endif

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComposer : IUserComposer
    {
        public void Compose(IBuilder builder)
        {
#if NETFRAMEWORK
            // Register settings
            builder.Register<VendrCheckoutSettings>(Lifetime.Singleton);

            // Register event handlers
            builder.AddVendrEventHandlers();

            // Register pipeline
            builder.AddVendrInstallPipeline();

            // Register services
            builder.Register<InstallService>(Lifetime.Singleton);

            // Register component
            builder.Components()
                .Append<VendrCheckoutComponent>();
#else
            // If Vendr Checkout hasn't been added manually by now, 
            // add it automatically with the default configuration.
            // If Vendr Checkout has already been added manully, then 
            // the AddVendrCheckout() call will just exit early.
            builder.AddVendrCheckout();
#endif
        }
    }
}