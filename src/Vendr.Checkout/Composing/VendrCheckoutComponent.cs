#if NETFRAMEWORK

using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.Cache;
using Vendr.Checkout.Events;
using Vendr.Core.Api;

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComponent : IComponent
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IVendrApi _vendrApi;

        public VendrCheckoutComponent(IUmbracoContextFactory umbracoContextFactory,
            IVendrApi vendrApi)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _vendrApi = vendrApi;
        }

        public void Initialize()
        {
            ContentCacheRefresher.CacheUpdated += (ContentCacheRefresher sender, CacheRefresherEventArgs e) =>
            {
                var handler = new SyncZeroValuePaymentProviderContinueUrl(_umbracoContextFactory, _vendrApi);

                handler.Handle(e.MessageObject, e.MessageType);
            };
        }

        public void Terminate()
        { }
    }
}

#endif