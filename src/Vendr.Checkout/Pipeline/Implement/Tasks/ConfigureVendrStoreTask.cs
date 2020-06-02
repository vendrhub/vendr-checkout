using Vendr.Core.Api;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    internal class ConfigureVendrStoreTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            using (var uow = VendrApi.Instance.Uow.Create())
            {
                // Set the store order editor config
                var store = VendrApi.Instance.GetStore(ctx.Store.Id)
                    .AsWritable(uow)
                    .SetOrderEditorConfig("~/app_plugins/vendrcheckout/config/vendrcheckout.order.editor.config.js");

                VendrApi.Instance.SaveStore(store);

                // Update order confirmation email
                var orderConfirmationEmailId = ctx.Store.ConfirmationEmailTemplateId;
                if (orderConfirmationEmailId.HasValue)
                {
                    var orderConfirmationEmail = VendrApi.Instance.GetEmailTemplate(orderConfirmationEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/app_plugins/vendrcheckout/views/emails/VendrCheckoutOrderConfirmationEmail.cshtml");

                    VendrApi.Instance.SaveEmailTemplate(orderConfirmationEmail);
                }

                // Update order confirmation email
                var orderErrorEmailId = ctx.Store.ErrorEmailTemplateId;
                if (orderErrorEmailId.HasValue)
                {
                    var orderErrorEmail = VendrApi.Instance.GetEmailTemplate(orderErrorEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/app_plugins/vendrcheckout/views/emails/VendrCheckoutOrderErrorEmail.cshtml");

                    VendrApi.Instance.SaveEmailTemplate(orderErrorEmail);
                }

                // Update gift card email
                var giftCardEmailId = ctx.Store.DefaultGiftCardEmailTemplateId;
                if (giftCardEmailId.HasValue)
                {
                    var giftCardEmail = VendrApi.Instance.GetEmailTemplate(giftCardEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/app_plugins/vendrcheckout/views/emails/VendrCheckoutGiftCardEmail.cshtml");

                    VendrApi.Instance.SaveEmailTemplate(giftCardEmail);
                }

                uow.Complete();
            }

            // Continue the pipeline
            return ctx;
        }
    }
}