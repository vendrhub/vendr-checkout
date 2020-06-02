using Umbraco.Core;
using Umbraco.Core.Composing;
using Vendr.Checkout.Events;
using Vendr.Core.Composing;
using Vendr.Core.Events.Notification;

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            // Register event handlers
            composition.WithNotificationEvent<OrderProductAddingNotification>()
                .RegisterHandler<OrderProductAddingHandler>();

            composition.WithNotificationEvent<OrderLineChangingNotification>()
                .RegisterHandler<OrderLineChangingHandler>();

            composition.WithNotificationEvent<OrderLineRemovingNotification>()
                .RegisterHandler<OrderLineRemovingHandler>();

            composition.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
                .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();

            composition.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
                .RegisterHandler<OrderShippingCountryRegionChangingHandler>();

            composition.WithNotificationEvent<OrderShippingMethodChangingNotification>()
                .RegisterHandler<OrderShippingMethodChangingHandler>();

            // Register component
            composition.Components()
                .Append<VendrCheckoutComponent>();
        }
    }
}