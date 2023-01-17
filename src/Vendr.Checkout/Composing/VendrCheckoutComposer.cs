using Umbraco.Cms.Core.Composing;
using Vendr.Umbraco;
using IBuilder = Umbraco.Cms.Core.DependencyInjection.IUmbracoBuilder;

namespace Vendr.Checkout.Composing
{
    [ComposeAfter(typeof(VendrComposer))]
    public class VendrCheckoutComposer : IComposer
    {
        public void Compose(IBuilder builder)
        {
            // If Vendr Checkout hasn't been added manually by now, 
            // add it automatically with the default configuration.
            // If Vendr Checkout has already been added manully, then 
            // the AddVendrCheckout() call will just exit early.
            builder.AddVendrCheckout();
        }
    }
}