using System.IO;
using Vendr.Common.IO;
using Vendr.Common.Pipelines;
using Vendr.Common.Pipelines.Tasks;
using Vendr.Core.Api;

namespace Vendr.Checkout.Pipeline.Tasks
{
    public class ConfigureVendrStoreTask : PipelineTaskBase<InstallPipelineContext>
    {
        private readonly IVendrApi _vendrApi;
        private readonly IIOHelper _ioHelper;

        public ConfigureVendrStoreTask(IVendrApi vendrApi,
            IIOHelper ioHelper)
        {
            _vendrApi = vendrApi;
            _ioHelper = ioHelper;
        }

        public override PipelineResult<InstallPipelineContext> Execute(PipelineArgs<InstallPipelineContext> args)
        {
            using (var uow = _vendrApi.Uow.Create())
            {
                // Create a store specific order editor config
                var configPath = _ioHelper.MapPath($"~/App_Plugins/Vendr/config/{args.Model.Store.Alias}.order.editor.config.js");
                if (!File.Exists(configPath))
                {
                    File.Copy(_ioHelper.MapPath($"~/App_Plugins/VendrCheckout/config/vendrcheckout.order.editor.config.js"),
                        configPath);
                }

                // Update order confirmation email
                var orderConfirmationEmailId = args.Model.Store.ConfirmationEmailTemplateId;
                if (orderConfirmationEmailId.HasValue)
                {
                    var orderConfirmationEmail = _vendrApi.GetEmailTemplate(orderConfirmationEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/App_Plugins/VendrCheckout/views/emails/VendrCheckoutOrderConfirmationEmail.cshtml");

                    _vendrApi.SaveEmailTemplate(orderConfirmationEmail);
                }

                // Update order confirmation email
                var orderErrorEmailId = args.Model.Store.ErrorEmailTemplateId;
                if (orderErrorEmailId.HasValue)
                {
                    var orderErrorEmail = _vendrApi.GetEmailTemplate(orderErrorEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/App_Plugins/VendrCheckout/views/emails/VendrCheckoutOrderErrorEmail.cshtml");

                    _vendrApi.SaveEmailTemplate(orderErrorEmail);
                }

                // Update gift card email
                var giftCardEmailId = args.Model.Store.DefaultGiftCardEmailTemplateId;
                if (giftCardEmailId.HasValue)
                {
                    var giftCardEmail = _vendrApi.GetEmailTemplate(giftCardEmailId.Value)
                        .AsWritable(uow)
                        .SetTemplateView("~/App_Plugins/VendrCheckout/views/emails/VendrCheckoutGiftCardEmail.cshtml");

                    _vendrApi.SaveEmailTemplate(giftCardEmail);
                }

                uow.Complete();
            }

            // Continue the pipeline
            return Ok();
        }
    }
}