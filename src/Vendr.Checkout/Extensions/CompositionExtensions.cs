using Vendr.Checkout.Events;
using Vendr.Checkout.Pipeline;
using Vendr.Checkout.Pipeline.Tasks;
using Vendr.Checkout.Web.Events.Notification.Handlers;
using Vendr.Core.Events.Notification;
using Vendr.Extensions;
using Vendr.Umbraco.Web.Events.Notification;

#if NETFRAMEWORK
using IBuilder = Umbraco.Core.Composing.Composition;
#else
using IBuilder = Umbraco.Cms.Core.DependencyInjection.IUmbracoBuilder;
#endif

namespace Vendr.Checkout.Extensions
{
    internal static class CompositionExtensions
    {
        public static IBuilder AddVendrEventHandlers(this IBuilder builder)
        {
            // Reset shipping / payment methods when certain elements of
            // an order change
            builder.WithNotificationEvent<OrderProductAddingNotification>()
                .RegisterHandler<OrderProductAddingHandler>();

            builder.WithNotificationEvent<OrderLineChangingNotification>()
                .RegisterHandler<OrderLineChangingHandler>();

            builder.WithNotificationEvent<OrderLineRemovingNotification>()
                .RegisterHandler<OrderLineRemovingHandler>();

            builder.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
                .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();

            builder.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
                .RegisterHandler<OrderShippingCountryRegionChangingHandler>();

            builder.WithNotificationEvent<OrderShippingMethodChangingNotification>()
                .RegisterHandler<OrderShippingMethodChangingHandler>();

            // Toggle order editor shipping address enabled flag based on
            // whether there vendr checkout is configured to collect a shipping address
            builder.WithNotificationEvent<OrderEditorConfigParsingNotification>()
                .RegisterHandler<VendrCheckoutOrderEditorConfigParsingNotificationHandler>();

            return builder;
        }

        public static IBuilder AddVendrInstallPipeline(this IBuilder builder)
        {
            builder.WithPipeline<InstallPipeline, InstallPipelineContext>()
                .Append<CreateVendrCheckoutDataTypesTask>()
                .Append<CreateVendrCheckoutDocumentTypesTask>()
                .Append<CreateVendrCheckoutNodesTask>()
                .Append<ConfigureVendrStoreTask>()
                .Append<CreateZeroValuePaymentMethodTask>();

            return builder;
        }
    }
}
