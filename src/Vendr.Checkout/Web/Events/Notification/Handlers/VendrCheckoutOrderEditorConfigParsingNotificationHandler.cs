using System.Linq;
using Vendr.Common.Events;
using Vendr.Umbraco.Web.Events.Notification;

#if NETFRAMEWORK
using Umbraco.Web;
#else
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
#endif

namespace Vendr.Checkout.Web.Events.Notification.Handlers
{
    public class VendrCheckoutOrderEditorConfigParsingNotificationHandler : NotificationEventHandlerBase<OrderEditorConfigParsingNotification>
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public VendrCheckoutOrderEditorConfigParsingNotificationHandler(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
        }

#if NETFRAMEWORK
        private IUmbracoContext UmbracoContext => _umbracoContextAccessor.UmbracoContext;
#else
        private IUmbracoContext UmbracoContext => _umbracoContextAccessor.GetRequiredUmbracoContext();
#endif

        public override void Handle(OrderEditorConfigParsingNotification evt)
        {
            if (evt.Config == null)
                return;

            var checkoutContentType = UmbracoContext.Content.GetContentType("vendrCheckoutCheckoutPage");
            var checkoutPages = UmbracoContext.Content.GetByContentType(checkoutContentType);
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