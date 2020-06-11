using System.Linq;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services.Changes;
using Umbraco.Web;
using Umbraco.Web.Cache;
using Vendr.Core.Api;
using Vendr.Core.Models;

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComponent : IComponent
    {
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public VendrCheckoutComponent(IUmbracoContextFactory umbracoContextFactory)
        {
            _umbracoContextFactory = umbracoContextFactory;
        }

        public void Initialize()
        {
            ContentCacheRefresher.CacheUpdated += SyncZeroValuePaymentProviderContinueUrl;
        }

        private void SyncZeroValuePaymentProviderContinueUrl(ContentCacheRefresher sender, CacheRefresherEventArgs e)
        {
            var payloads = e.MessageObject as ContentCacheRefresher.JsonPayload[];
            if (payloads == null)
                return;

            using (var umbNew = _umbracoContextFactory.EnsureUmbracoContext())
            {
                foreach (var payload in payloads)
                {
                    if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshNode))
                    {
                        // Single node refresh

                        var node = umbNew.UmbracoContext.Content.GetById(payload.Id);
                        if (node != null && IsConfirmationPageType(node))
                        {
                            SyncZeroValuePaymentProviderContinueUrl(node);
                        }
                    }
                    else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshBranch))
                    {
                        // Branch refresh

                        var rootNode = umbNew.UmbracoContext.Content.GetById(payload.Id);
                        if (rootNode != null)
                        {
                            var nodeType = umbNew.UmbracoContext.Content.GetContentType(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);
                            if (nodeType == null)
                                continue;

                            var nodes = umbNew.UmbracoContext.Content.GetByContentType(nodeType);

                            foreach (var node in nodes?.Where(x => IsConfirmationPageType(x) && x.Path.StartsWith(rootNode.Path)))
                            {
                                SyncZeroValuePaymentProviderContinueUrl(node);
                            }
                        }
                    }
                    else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshAll)) 
                    {
                        // All refresh

                        var nodeType = umbNew.UmbracoContext.Content.GetContentType(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);
                        if (nodeType == null)
                            continue;

                        var nodes = umbNew.UmbracoContext.Content.GetByContentType(nodeType);

                        foreach (var node in nodes?.Where(x => IsConfirmationPageType(x)))
                        {
                            SyncZeroValuePaymentProviderContinueUrl(node);
                        }
                    }
                }
            }
        }

        private bool IsConfirmationPageType(IPublishedContent node)
        {
            if (node == null || node.ContentType == null || !node.HasProperty("vendrStepType"))
                return false;

            return node.ContentType.Alias == VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage && node.Value<string>("vendrStepType") == "Confirmation";
        }

        private void SyncZeroValuePaymentProviderContinueUrl(IPublishedContent confirmationNode)
        {
            if (confirmationNode == null) 
                return;

            var store = confirmationNode.Value<StoreReadOnly>(Core.Constants.Properties.StorePropertyAlias, fallback: Fallback.ToAncestors);
            if (store == null)
                return;

            var paymentMethod = VendrApi.Instance.GetPaymentMethod(store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue);
            if (paymentMethod == null)
                return;

            using (var uow = VendrApi.Instance.Uow.Create())
            {
                var writable = paymentMethod.AsWritable(uow)
                    .SetSetting("ContinueUrl", confirmationNode.Url());

                VendrApi.Instance.SavePaymentMethod(writable);

                uow.Complete();
            }
        }

        public void Terminate()
        { }
    }
}