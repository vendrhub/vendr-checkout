using System;
using System.Linq;
using Vendr.Core.Models;
using Vendr.Core.Api;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Core.Services.Changes;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Vendr.Checkout.Events
{
    public class SyncZeroValuePaymentProviderContinueUrl : INotificationHandler<ContentCacheRefresherNotification>
    {
        public void Handle(ContentCacheRefresherNotification notification)
        {
            Handle(notification.MessageObject, notification.MessageType);
        }

        private IUmbracoContextFactory _umbracoContextFactory;
        private IVendrApi _vendrApi;

        public SyncZeroValuePaymentProviderContinueUrl(IUmbracoContextFactory umbracoContextFactory,
            IVendrApi vendrApi)
        {
            _umbracoContextFactory = umbracoContextFactory;
            _vendrApi = vendrApi;
        }

        public void Handle(object messageObject, MessageType messageType)
        {
            if (messageType != MessageType.RefreshByPayload)
                throw new NotSupportedException();

            var payloads = messageObject as ContentCacheRefresher.JsonPayload[];
            if (payloads == null)
                return;

            using (var ctx = _umbracoContextFactory.EnsureUmbracoContext())
            {
                foreach (var payload in payloads)
                {
                    if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshNode))
                    {
                        // Single node refresh
                        var node = ctx.UmbracoContext.Content.GetById(payload.Id);
                        if (node != null && IsConfirmationPageType(node))
                        {
                            DoSyncZeroValuePaymentProviderContinueUrl(node);
                        }
                    }
                    else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshBranch))
                    {
                        // Branch refresh
                        var rootNode = ctx.UmbracoContext.Content.GetById(payload.Id);
                        if (rootNode != null)
                        {
                            var nodeType = ctx.UmbracoContext.Content.GetContentType(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);
                            if (nodeType == null)
                                continue;

                            var nodes = ctx.UmbracoContext.Content.GetByContentType(nodeType);

                            foreach (var node in nodes?.Where(x => IsConfirmationPageType(x) && x.Path.StartsWith(rootNode.Path)))
                            {
                                DoSyncZeroValuePaymentProviderContinueUrl(node);
                            }
                        }
                    }
                    else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshAll))
                    {
                        // All refresh
                        var nodeType = ctx.UmbracoContext.Content.GetContentType(VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage);
                        if (nodeType == null)
                            continue;

                        var nodes = ctx.UmbracoContext.Content.GetByContentType(nodeType);

                        foreach (var node in nodes?.Where(x => IsConfirmationPageType(x)))
                        {
                            DoSyncZeroValuePaymentProviderContinueUrl(node);
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

        private void DoSyncZeroValuePaymentProviderContinueUrl(IPublishedContent confirmationNode)
        {
            if (confirmationNode == null)
                return;

            var store = confirmationNode.Value<StoreReadOnly>(Umbraco.Constants.Properties.StorePropertyAlias, fallback: Fallback.ToAncestors);
            if (store == null)
                return;

            var paymentMethod = _vendrApi.GetPaymentMethod(store.Id, VendrCheckoutConstants.PaymentMethods.Aliases.ZeroValue);
            if (paymentMethod == null)
                return;

            using (var uow = _vendrApi.Uow.Create())
            {
                var writable = paymentMethod.AsWritable(uow)
                    .SetSetting("ContinueUrl", confirmationNode.Url());

                _vendrApi.SavePaymentMethod(writable);

                uow.Complete();
            }
        }
    }
}
