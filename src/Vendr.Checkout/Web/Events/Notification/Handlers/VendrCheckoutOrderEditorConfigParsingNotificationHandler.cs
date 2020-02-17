using System.Linq;
using Umbraco.Web;
using Umbraco.Web.Composing;
using Vendr.Core.Events.Notification;
using Vendr.Web.Events.Notification;

namespace Vendr.Checkout.Web.Events.Notification.Handlers
{
    public class VendrCheckoutOrderEditorConfigParsingNotificationHandler : NotificationEventHandlerBase<OrderEditorConfigParsingNotification>
    {
        public override void Handle(OrderEditorConfigParsingNotification evt)
        {
            if (evt.Config == null)
                return;

            var checkoutContentType = Current.UmbracoContext.Content.GetContentType("vendrCheckoutCheckoutPage");
            var checkoutPages = Current.UmbracoContext.Content.GetByContentType(checkoutContentType);
            var checkoutPage = checkoutPages.FirstOrDefault(x => x.GetStore()?.Id == evt.StoreId);

            if (checkoutPage != null)
            {
                // Extract config settings
                var vendrCollectShippingInfo = checkoutPage.Value<bool>("vendrCollectShippingInfo");

                // Update parsed config
                evt.Config["shipping"]["enabled"] = vendrCollectShippingInfo;
            }
        }
    }
}