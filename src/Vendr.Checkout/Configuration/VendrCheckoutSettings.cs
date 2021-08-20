#if NETFRAMEWORK
using System.Configuration;
#endif

namespace Vendr.Checkout.Configuration
{
    public class VendrCheckoutSettings
    {
        public string RootViewPath { get; set; }

        public bool ResetPaymentMethodOnShippingMethodChange { get; set; }

        public VendrCheckoutSettings()
        {
            RootViewPath = "/App_Plugins/VendrCheckout";

#if NETFRAMEWORK
            var configRootViewPath = ConfigurationManager.AppSettings["VendrCheckout:RootViewPath"]?.ToString();
            if (!string.IsNullOrWhiteSpace(configRootViewPath))
                RootViewPath = configRootViewPath;

            ResetPaymentMethodOnShippingMethodChange = ConfigurationManager.AppSettings["VendrCheckout:ResetPaymentMethodOnShippingMethodChange"] != "false";
#endif
        }
    }
}
