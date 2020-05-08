using Umbraco.Core.Composing;
using Vendr.Checkout.Web.Events.Notification.Handlers;
using Vendr.Core.Composing;
using Vendr.Web.Events.Notification;

namespace Vendr.Checkout.Web.Composing
{
    public class VendrCheckoutWebComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.WithNotificationEvent<OrderEditorConfigParsingNotification>()
                .RegisterHandler<VendrCheckoutOrderEditorConfigParsingNotificationHandler>();
        }
    }
}